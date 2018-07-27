namespace navdi2.grid {
	using UnityEngine;
	public class DataGrid<T> {
		public DataGrid(NavdiGrid grid) {
			this.options = grid.options;
			InitializeCells();
		}
		public DataGrid(GridOptions options) {
			this.options = options;
			InitializeCells();
		}

		//// PUBLIC-ACCESS FUNCTIONS, same as NavdiGrid has
		public T GetCell(int x, int y) { return cells[x,y]; }
		public T GetCell(twin cellPos) { return cells[cellPos.x, cellPos.y]; }
		public void SetCell(int x, int y, T value) { cells[x,y] = value; }
		public void SetCell(twin cellPos, T value) { cells[cellPos.x, cellPos.y] = value; }
		public void DoEach(System.Action<twin> twinFunction) {
			for(int y=options.size_cellsInGrid.y-1;y>=0;y--)for(int x=0;x<options.size_cellsInGrid.x;x++){
				twinFunction(new twin(x,y));
			}
		}
		public void DoEach(System.Action<int,int> xyFunction) {
			for(int y=options.size_cellsInGrid.y-1;y>=0;y--)for(int x=0;x<options.size_cellsInGrid.x;x++){
				xyFunction(x,y);
			}
		}
		public void DoEach(System.Action<int,int,T> xycFunction) {
			for(int y=options.size_cellsInGrid.y-1;y>=0;y--)for(int x=0;x<options.size_cellsInGrid.x;x++){
				xycFunction(x,y,cells[x,y]);
			}
		}
		public bool CellPosInBounds(twin cellPos) {
			return cellPos>=twin.zero&&cellPos<options.size_cellsInGrid;
		}

		//// INTERNAL FUNCTIONS
		void InitializeCells() {
			this.cells = new T[options.size_cellsInGrid.x, options.size_cellsInGrid.y];
		}

		//// INTERNAL PROPERTIES
		GridOptions options;
		twin size {get{return options.size_cellsInGrid;}}
		T[,] cells;
		
	}
}