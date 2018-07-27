namespace navdi2.grid {
	using UnityEngine;
	public struct GridOptions {
		public string name;
		public Bank entityBank;
		public twin size_cellsInGrid;
		public twin size_pixelsPerCell;
		public twin origin;
		public Vector2 calign_grid;
		public Vector2 calign_cell;
		public GridOptions(
			string name,
			Bank entityBank,
			twin size_cellsInGrid,
			twin size_pixelsPerCell,
			twin origin = new twin(),
			Vector2 calign_grid = new Vector2(),
			Vector2 calign_cell = new Vector2()
		) {
			this.name = name;
			this.entityBank = entityBank;
			this.size_cellsInGrid = size_cellsInGrid;
			this.size_pixelsPerCell = size_pixelsPerCell;
			this.origin = origin;
			this.calign_grid = calign_grid;
			this.calign_cell = calign_cell;
		}

		//// figured values
		public int count_cellsInGrid { get { return size_cellsInGrid.x * size_cellsInGrid.y; } }
		
	}
}