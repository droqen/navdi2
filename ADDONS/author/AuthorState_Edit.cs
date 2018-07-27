namespace navdi2.ADDONS.author {
	using navdi2.grid;
	using System.Collections.Generic;
	public class AuthorState_Edit:State {
		AuthorXXI xxi{get{return AuthorXXI.ins;}}
		NavdiGrid editGrid;
		Ents<AuthorCursor> cursors;
		twin[] cursorPositions;
		public AuthorState_Edit() : base() {
			Add(editGrid = new NavdiGrid(new GridOptions(
				name: "authorstate_edit_grid",
				entityBank: xxi.authorCellBank,
				size_cellsInGrid: new twin(xxi.options.size_gridScreen / xxi.options.size_pixelsInCell),
				size_pixelsPerCell: new twin(xxi.options.size_pixelsInCell)
			)));
			cursorPositions = null;
			Add(cursors = new Ents<AuthorCursor>());
		}

		override public void Activated() {
			base.Activated();
			cursorPositions = null;
			xxi.LoadRoom(editGrid);
		}

		override public void FrequentStep(){

			base.FrequentStep();
			
			twin cursorPos = editGrid.PointToCellPos(xxi.pcam.MouseWorldPoint());

			if (xxi.Input_MouseDown_LeftButton() && editGrid.CellPosInBounds(cursorPos)) {
				if (cursorPositions == null) cursorPositions = MagicWand(cursorPos);
				else cursorPositions = null;
			}
			if (cursorPositions == null) UpdateCursors(cursorPos);

			twin dir;
			if (xxi.Input_KeyDown_PushToggle()) {
				xxi.SaveRoom(editGrid);
				xxi.SetState(xxi.state_play);
			} else if (xxi.Input_Key_ShowPalette()) {
				xxi.SaveRoom(editGrid);
				xxi.SetState(xxi.state_editpalette);
			} else if (xxi.Input_KeyDown_Dir(out dir)) {
				MoveRoomCoord(dir);
			} else {
				ushort cellValue;
				if (xxi.Input_Key_CellValue(out cellValue)) {
					if (cursorPositions != null) { // multi select cursor
						foreach(twin pos in cursorPositions) {
							Paint(pos, cellValue);
						}
					} else { // basic hover cursor
						Paint(cursorPos, cellValue);
					}
				}
			}
		}

		//// INTERNAL
		void Paint(twin cellPos, ushort cellValue) {
			if (editGrid.CellPosInBounds(cellPos)) {
				AuthorCell cell = (AuthorCell)editGrid.GetCell(cellPos);
				if (cell!=null && cell.GetCellType(cellValue) != (byte)AuthorCell_SuperType.Unused) {
					cell.SetValue(cellValue);
				}
			}
		}
		void UpdateCursors(params twin[] cursorPoses) {
			cursors.Clear();
			foreach(twin pos in cursorPoses) {
				cursors.Add(xxi.authorCursorBank.Spawn<AuthorCursor>().Setup(position: editGrid.CellPosToPoint(pos), fill:true));
			}
		}
		twin[] MagicWand(twin startingCellPos) {
			List<twin> wandedPositions = new List<twin>();
			wandedPositions.Add(startingCellPos);
			ushort magicValue = editGrid.GetCell(startingCellPos).value;
			for (int i = 0; i < wandedPositions.Count; i++) {
				twin here = wandedPositions[i];
				twin[] ns = new twin[4];
				ns[0] = here + twin.right;
				ns[1] = here + twin.up;
				ns[2] = here + twin.left;
				ns[3] = here + twin.down;
				foreach(twin n in ns)
					if (editGrid.CellPosInBounds(n) && editGrid.GetCell(n).value == magicValue && !wandedPositions.Contains(n))
						wandedPositions.Add(n);
			}
			if (wandedPositions.Count <= 1) return null;
			twin[] poses = wandedPositions.ToArray();
			UpdateCursors(poses);
			return poses;
		}
		void MoveRoomCoord(twin dir) {
			ushort[] edge = CopyGridsEdge(dir, editGrid);
			xxi.SetRoomCoord(xxi.currentRoomCoord + dir, editGrid);
			if (edge != null) PasteGridsEdge(edge, -dir, editGrid);
		}

		//// COPY/PASTE FUNCTIONS
		ushort[] CopyGridsEdge(twin dirEdge, NavdiGrid grid) {
			if ( (dirEdge.x==0) == (dirEdge.y==0) ) return null;
			bool copyColumn = (dirEdge.x!=0);
			ushort[] edge;
			if (copyColumn) edge = CopyCol(dirEdge.x<0?0:-1, grid);
			else edge = CopyRow(dirEdge.y<0?0:-1, grid);
			return edge;
		}
		void PasteGridsEdge(ushort[] edgeData, twin dirEdge, NavdiGrid grid) {
			if ( (dirEdge.x==0) == (dirEdge.y==0) ) return;
			bool pasteColumn = (dirEdge.x!=0);
			if (pasteColumn) PasteCol(edgeData, dirEdge.x<0?0:-1, grid);
			else PasteRow(edgeData, dirEdge.y<0?0:-1, grid);
		}
		ushort[] CopyRow(int row, NavdiGrid grid) {
			if (row < 0) row = grid.options.size_cellsInGrid.y + row;
			ushort[] edge = new ushort[grid.options.size_cellsInGrid.x];
			for (int x = 0; x < edge.Length; x++) edge[x] = grid.GetCell(x,row).value;
			return edge;
		}
		ushort[] CopyCol(int col, NavdiGrid grid) {
			if (col < 0) col = grid.options.size_cellsInGrid.x + col;
			ushort[] edge = new ushort[grid.options.size_cellsInGrid.y];
			for (int y = 0; y < edge.Length; y++) edge[y] = grid.GetCell(col,y).value;
			return edge;
		}
		void PasteRow(ushort[] edgeData, int row, NavdiGrid grid) {
			if (row < 0) row = grid.options.size_cellsInGrid.y + row;
			for (int x = 0; x < edgeData.Length; x++) grid.GetCell(x,row).SetValue(edgeData[x]);
		}
		void PasteCol(ushort[] edgeData, int col, NavdiGrid grid) {
			if (col < 0) col = grid.options.size_cellsInGrid.x + col;
			for (int y = 0; y < edgeData.Length; y++) grid.GetCell(col,y).SetValue(edgeData[y]);
		}
	}
}