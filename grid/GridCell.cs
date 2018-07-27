namespace navdi2.grid {
	public class GridCell : Bod {
		public NavdiGrid grid{get;private set;}
		public twin cellPos{get;private set;}
		public ushort value{get;private set;}
		public GridCell Setup_RequiresGrid(NavdiGrid grid, twin cellPos) {
			base.Setup();
			this.grid=grid;
			this.SetCellPos(cellPos);
			// this.SetValue(0);
			return this;
		}
		override public void Activated() {
			base.Activated();
			this.SetValue(0);
		}
		public void SetValue(ushort value, bool refresh=true){
			this.value=value;
			if (refresh) RefreshValue();
		}
		////VIRTUAL
		virtual public void RefreshValue() {
			// pls override this default behaviour
			this.spriter.enabled = value > 0;
			this.collides = value > 0;
		}
		////INTERNAL
		void SetCellPos(twin cellPos){
			this.cellPos=cellPos;
			this.position=this.grid.CellPosToPoint(cellPos);
		}
	}
}