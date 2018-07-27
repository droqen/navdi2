namespace navdi2.grid {
	using System.Collections.Generic;
	public class DataGridFloodPath : DataGrid<int> {
		public DataGridFloodPath(
		NavdiGrid grid,
		System.Func<twin,bool> GetCanEnter,
		System.Func<twin,byte> GetCostToEnter,
		params twin[] targets) : base(grid) {
			this.GetCanEnter = GetCanEnter;
			this.GetCostToEnter = GetCostToEnter;
			InitializeFloodPath(targets);
		}

		//// PUBLIC FUNCTION

		public int GetDistance(twin cellPos) {
			return GetCell(cellPos);
		}
		public twin[] GetDownhillDirs(twin cellPos, bool shuffled = true) {
			int mostDownhill = int.MaxValue;
			List<twin> downhillDirs = new List<twin>();

			twin.StraightenCompass();
			foreach(twin dir in twin.compass) CheckDownhill(cellPos, dir, ref mostDownhill, ref downhillDirs);
			CheckDownhill(cellPos, twin.zero, ref mostDownhill, ref downhillDirs);

			twin[] downhillDirArray = downhillDirs.ToArray();
			if (shuffled) Util.Shuffle<twin>(ref downhillDirArray);
			return downhillDirArray;
		}


		//// INT. FUNC'S

		void CheckDownhill(twin cellPos, twin dir, ref int mostDownhill, ref List<twin> downhillDirs) {
			if (CellPosInBounds(cellPos + dir)) {
				int height = GetCell(cellPos + dir);
				if (height < mostDownhill) {
					mostDownhill = height;
					downhillDirs.Clear();
					downhillDirs.Add(dir);
				} else if (height == mostDownhill) {
					downhillDirs.Add(dir);
				} // else: pass
			}
		}

		void InitializeFloodPath(params twin[] targets) {
			DoEach((x,y)=>{SetCell(x,y,int.MaxValue);});

			floodPathIndex = 0;
			floodPathQueue = new List<twin>();
			foreach(twin target in targets) FloodPathAdd(target, 0);

			twin.StraightenCompass();
			while (floodPathIndex < floodPathQueue.Count) FloodPathIter();
		}
		void FloodPathIter() {
			twin cellPos = floodPathQueue[floodPathIndex];
			if (GetCanEnter(cellPos)) {
				int uphillDistance = GetCell(cellPos) + 1 + GetCostToEnter(cellPos);
				foreach(twin dir in twin.compass) { // see function 'InitializeFloodPath', 'twin.StraightenCompass()' is called up there to save power
					FloodPathAdd(cellPos+dir,uphillDistance);
				}
			}
			floodPathIndex++; // always count up.
		}
		void FloodPathAdd(twin nextPos, int distance) {
			if (CellPosInBounds(nextPos) && !floodPathQueue.Contains(nextPos) &&  GetCell(nextPos) > distance) {
				SetCell(nextPos, distance);
				floodPathQueue.Add(nextPos);
			}
		}

		//// INTERNAL
		System.Func<twin,bool> GetCanEnter;
		System.Func<twin,byte> GetCostToEnter;
		int floodPathIndex;
		List<twin> floodPathQueue;
	}
}