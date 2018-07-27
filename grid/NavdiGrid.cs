namespace navdi2.grid {
	using UnityEngine;
	public class NavdiGrid : Ents<GridCell>, IBodCollidable {
		public GridOptions options {get;private set;}
		GridCell[,] cells;
		public NavdiGrid(GridOptions options) : base(maxSize:options.count_cellsInGrid) {
			this.options = options;
		}
		override public void Activated() {
			base.Activated();
			this.RebuildGrid();
		}
		override public void Deactivated() {
			base.Deactivated();
			this.cells=null;
		}

		//// IBodCollidable
		public Bod HitRect(Vector2 pos, Vector2 size){
			if(!this.active)return null;

			Bod hit;
			twin min=PointToCellPos(pos);
			twin max=PointToCellPos(pos+size);
			for(int x=min.x;x<=max.x;x++)for(int y=min.y;y<=max.y;y++){
				if(!CellPosInBounds(new twin(x,y)))continue;
				hit=cells[x,y].HitRect(pos,size);
				if(hit!=null)return hit;
			}

			return null;
		}

		////PUBLIC: Modifying/controlling cells
		public GridCell GetCell(int x, int y) { return cells[x,y]; }
		public GridCell GetCell(twin cellPos) { return cells[cellPos.x, cellPos.y]; }
		public void DoEach(System.Action<int,int> xyFunction) {
			// for(int x=0;x<options.size_cellsInGrid.x;x++)for(int y=0;y<options.size_cellsInGrid.y;y++){
			for(int y=options.size_cellsInGrid.y-1;y>=0;y--)for(int x=0;x<options.size_cellsInGrid.x;x++){
				xyFunction(x,y);
			}
		}
		public void DoEach(System.Action<int,int,GridCell> xycFunction) {
			// for(int x=0;x<options.size_cellsInGrid.x;x++)for(int y=0;y<options.size_cellsInGrid.y;y++){
			for(int y=options.size_cellsInGrid.y-1;y>=0;y--)for(int x=0;x<options.size_cellsInGrid.x;x++){
				xycFunction(x,y,cells[x,y]);
			}
		}

		////IMPORT-EXPORT FUNCTIONS
		public void ImportString(string encodedGrid, char separator){
			RebuildGrid(); // not sure if necessary
			string[] valueStrings=encodedGrid.Split(separator);
			if (valueStrings.Length!=options.count_cellsInGrid) 
				throw Dj.Crashf("ImportString misalignment. Got {0} values, expected {1} values ({2}x{3} grid)",
				valueStrings.Length,options.count_cellsInGrid,
				options.size_cellsInGrid.x,options.size_cellsInGrid.y);
			int i=0;
			DoEach((x,y)=>{
				cells[x,y].SetValue(ushort.Parse(valueStrings[i++]));
			});
		}
		public string ExportString(string separator){
			string[] valueStrings=new string[options.count_cellsInGrid];
			int i=0;
			DoEach((x,y)=>{
				valueStrings[i++]=cells[x,y].value.ToString();
			});
			return string.Join(separator,valueStrings);
		}

		////MATH FUNCTIONS
		public bool CellPosInBounds(twin cellPos) {
			return cellPos>=twin.zero&&cellPos<options.size_cellsInGrid;
		}
		public twin PointToCellPos(Vector2 point) {
			return new twin(point) / options.size_pixelsPerCell; // round point, divide it by cell size.
		}
		public Vector2 CellPosToPoint(twin cellPos) {
			return cellPos * options.size_pixelsPerCell;
		}

		////INTERNAL
		void RebuildGrid(){
			Clear();
			this.cells=new GridCell[options.size_cellsInGrid.x,options.size_cellsInGrid.y];
			DoEach((x,y)=>{	cells[x,y]=options.entityBank.Spawn<GridCell>(); });
			DoEach((x,y)=>{	cells[x,y].Setup_RequiresGrid(this, new twin(x,y)); });
			DoEach((x,y)=>{	Add(cells[x,y]); });
		}
	}
}