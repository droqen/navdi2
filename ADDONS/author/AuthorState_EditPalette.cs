namespace navdi2.ADDONS.author {
	using navdi2.grid;
	using UnityEngine;
	public class AuthorState_EditPalette:State {
		AuthorXXI xxi{get{return AuthorXXI.ins;}}
		NavdiGrid paletteGrid;
		Ents<AuthorCursor> cursors;
		public AuthorState_EditPalette() : base() {
			Add(paletteGrid = new NavdiGrid(new GridOptions(
				name: "authorstate_editpalette_grid",
				entityBank: xxi.authorCellBank,
				size_cellsInGrid: xxi.options.size_palette,
				size_pixelsPerCell: xxi.options.size_gridScreen / xxi.options.size_palette
			)));
			Add(cursors = new Ents<AuthorCursor>(1));
		}

		override public void Activated() {
			base.Activated();
			ushort nextCellValue = 0;
			paletteGrid.DoEach((x,y,c)=>{c.SetValue(nextCellValue++);});
			cursors.Add(xxi.authorCursorBank.Spawn<AuthorCursor>().Setup(outline:true));
			UpdateCursorPos();
		}
		override public void FrequentStep(){
			base.FrequentStep();

			twin cursorPos = UpdateCursorPos();
			
			if (!xxi.Input_Key_ShowPalette()) {
				xxi.SetState(xxi.state_edit);
			} else {
				KeyCode k = xxi.keys.GetCurrentKey();
				if (paletteGrid.CellPosInBounds(cursorPos)) {
					AuthorCell cell = (AuthorCell)paletteGrid.GetCell(cursorPos);
					bool cellSelectable = (cell!=null&&cell.type!=(byte)AuthorCell_SuperType.Unused);
					bool inputIsNumber = (k >= KeyCode.Alpha0 && k <= KeyCode.Alpha9);
					bool inputIsAlpha = (k >= KeyCode.A && k <= KeyCode.Z);

					if (cellSelectable && (inputIsNumber||inputIsAlpha))
						xxi.Input_SetKeyCellValue(k, cell.value);
				}
			}
		}

		//// INTERNAL
		twin UpdateCursorPos() {
			twin mouseCellPos = paletteGrid.PointToCellPos(xxi.pcam.MouseWorldPoint());
			if (paletteGrid.CellPosInBounds(mouseCellPos) && ((AuthorCell)paletteGrid.GetCell(mouseCellPos)).type != (byte)AuthorCell_SuperType.Unused) {
				this.cursors.DoEach((cursor)=>{cursor.spriter.enabled=true; cursor.position = paletteGrid.CellPosToPoint(mouseCellPos);});
			} else {
				this.cursors.DoEach((cursor)=>{cursor.spriter.enabled=false;});
			}
			return mouseCellPos;
		}
	}
}