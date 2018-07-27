namespace navdi2.ADDONS.author {
	public struct AuthorXXIOptions {
		public twin size_gridScreen;
		public twin size_pixelsInCell;
		public twin pos_gridScreen;
		public twin size_pixelsInScreen { get { return size_gridScreen * size_pixelsInCell; } }
		public twin size_palette;
		public twin pos_startingRoom;
		public char separator_grid_cells;
		public char separator_grid_codes;
		public AuthorXXIOptions(
			twin size_gridScreen,
			twin size_pixelsInCell,
			twin pos_gridScreen=default(twin),
			twin size_palette=default(twin),
			twin pos_startingRoom=default(twin),
			char separator_grid_cells=',',
			char separator_grid_codes='×'
		) {
			this.size_gridScreen=size_gridScreen;
			this.size_pixelsInCell=size_pixelsInCell;
			this.pos_gridScreen=pos_gridScreen;
			if (size_palette>twin.zero) this.size_palette=size_palette; else this.size_palette=new twin(10,10);
			this.pos_startingRoom=pos_startingRoom;
			this.separator_grid_cells = separator_grid_cells;
			this.separator_grid_codes = separator_grid_codes;
		}
	}
}