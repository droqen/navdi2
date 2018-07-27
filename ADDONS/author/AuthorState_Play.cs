namespace navdi2.ADDONS.author {
	using navdi2.grid;
	using UnityEngine;
	public class AuthorState_Play:State {
		AuthorXXI xxi{get{return AuthorXXI.ins;}}
		public NavdiGrid playGrid{get;private set;}
		protected Ents<Ents> entsSingleScreen{get;private set;}
		protected Ents<Ents> entsAcrossScreens{get;private set;}
		public AuthorState_Play() : base() {
			Add(this.playGrid = new NavdiGrid(new GridOptions(
				name: "authorstate_play_grid",
				entityBank: xxi.authorCellBank,
				size_cellsInGrid: xxi.options.size_gridScreen / xxi.options.size_pixelsInCell,
				size_pixelsPerCell: new twin(xxi.options.size_pixelsInCell)
			)));
			Add(this.entsSingleScreen = new Ents<Ents>(permanent:true));
			Add(this.entsAcrossScreens = new Ents<Ents>(permanent:true));
		}

		override public void Activated() {
			base.Activated();
			LoadRoom();
		}
		override public void FrequentStep() {
			base.FrequentStep();

			if (xxi.Input_KeyDown_PushToggle()) xxi.SetState(xxi.state_edit);
		}

		//// PUBLIC: REGISTERING ENTS
		public void RegisterEnts(Ents ents, bool keepAcrossScreens = false) {
			(keepAcrossScreens?entsAcrossScreens:entsSingleScreen).Add(ents);
		}

		//// PUBLIC: SPAWNING ENTS, MOVING ROOMS
		public B Spawn<B>(Ents ents, Bank bank) where B:Bod {
			B bod = bank.Spawn<B>();
			ents.Add(bod);
			return bod;
		}
		virtual public void MoveToRoomCoord(twin roomCoord) {
			entsSingleScreen.Clear();
			xxi.SetRoomCoord(roomCoord, playGrid);
			playGrid.DoEach((cell)=>{((AuthorCell)cell).SetToPlayMode(this);}); // set all cells to 'Play' mode, generally meaning call Spawn functions
		}

		//// INTERNAL
		void LoadRoom() {
			xxi.LoadRoom(playGrid);
			playGrid.DoEach((cell)=>{((AuthorCell)cell).SetToPlayMode(this);}); // set all cells to 'Play' mode, generally meaning call Spawn functions
		}
	}
}