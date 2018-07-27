namespace navdi2.ADDONS.author {
	using navdi2.grid;
	using navdi2.file;
	using UnityEngine;
	using System.Collections.Generic;
	public class AuthorXXI:XXI {

		//// PROPERTIES (inspector)
		[Header("AuthorXXI Required Values")]
		public navdi2.mono.PerfectCam pcam;
		public navdi2.mono.KeyStackWatcher keys;
		public string roomsFolderName = "DEFAULT";
		[SerializeField] AuthorCell authorCell;
		[SerializeField] AuthorCursor authorCursor;

		//// PROPERTIES (options and banks etc)
		public AuthorXXIOptions options {get;private set;}
		public Bank authorCellBank {get;private set;}
		public Bank authorCursorBank {get;private set;}

		//// PROPERTIES ('current grid string' etc)
		string currentRoomGridString;
		public twin currentRoomCoord {get;private set;}

		//// INTERNAL PROPERTIES (input)
		Dictionary<KeyCode,ushort> keyToCellValue;

		//// PROPERTIES (states)
		public AuthorState_Play state_play;
		public AuthorState_Edit state_edit;
		public AuthorState_EditPalette state_editpalette;

		//// XXI
		public static AuthorXXI ins;
		override protected void Initialize() {
			ins = this;

			this.options = InitializeOptions();
			if (authorCell == null) throw Dj.Crash("AuthorXXI requires [authorCell] to be set in inspector");
			this.authorCellBank = GetBank(authorCell.name);
			if (authorCursor == null) throw Dj.Crash("AuthorXXI requires [authorCursor] to be set in inspector");
			this.authorCursorBank = GetBank(authorCursor.name);

			this.currentRoomGridString = "";
			SetRoomCoord(options.pos_startingRoom, null);

			this.keyToCellValue = new Dictionary<KeyCode, ushort>();
			Input_SetKeyCellValue(KeyCode.Q, 0);
			for (int i = 0; i < 9; i++) Input_SetKeyCellValue(KeyCode.Alpha1+i, (ushort)(1+i));

			this.InitializeStates();
			if (this.state_play == null) throw Dj.Crash(name+".state_play is null. did you override InitializeStates badly?");
			if (this.state_edit == null) throw Dj.Crash(name+".state_edit is null. did you override InitializeStates badly?");
			if (this.state_editpalette == null) throw Dj.Crash(name+".state_editpalette is null. did you override InitializeStates badly?");
			SetState(state_edit);
		}
		//// VIRTUAL: OVERRIDE TO CUSTOMIZE AUTHOR XXI OPTIONS
		virtual protected AuthorXXIOptions InitializeOptions() {
			return new AuthorXXIOptions(
				size_gridScreen: new twin(160,144),
				size_pixelsInCell: new twin(8,8)
			);
		}
		virtual protected void InitializeStates() {
			this.state_play = new AuthorState_Play();
			this.state_edit = new AuthorState_Edit();
			this.state_editpalette = new AuthorState_EditPalette();
		}
		//// PUBLIC: 'CURRENT ROOM' functions
		public void SetRoomCoord(twin roomCoord, NavdiGrid grid) {
			if (grid != null) SaveRoom(grid);
			this.currentRoomCoord = roomCoord;
			if (grid != null) LoadRoom(grid);
		}
		string GetRoomCoordString(twin roomCoord) {
			return ""+this.currentRoomCoord.x+options.separator_grid_cells+this.currentRoomCoord.y;
		}
		public void SaveRoom(NavdiGrid grid) {
			this.currentRoomGridString = grid.ExportString(""+options.separator_grid_cells);
			File.CreateFolderIfDoesNotExist(
				File.CombinedPath(Application.streamingAssetsPath, roomsFolderName)
			);
			File.WriteFile(
				File.CombinedPath(Application.streamingAssetsPath, roomsFolderName, this.GetRoomCoordString(this.currentRoomCoord)),
				this.currentRoomGridString
			);
		}
		public void LoadRoom(NavdiGrid grid) {
			File.CreateFolderIfDoesNotExist(
				File.CombinedPath(Application.streamingAssetsPath, roomsFolderName)
			);
			string levelPath = File.CombinedPath(Application.streamingAssetsPath, roomsFolderName, this.GetRoomCoordString(this.currentRoomCoord));
			if (File.DoesFileExist(levelPath)) {
				this.currentRoomGridString = File.ReadFile(levelPath);
				grid.ImportString(this.currentRoomGridString, options.separator_grid_cells);
			} else {
				grid.DoEach((cell)=>{cell.SetValue(0);});
			}
		}
		//// PUBLIC: CONTROLS
		public void Input_SetKeyCellValue(KeyCode k, ushort cellValue) {
			this.keyToCellValue[k] = cellValue;
		}
		public bool Input_MouseDown_LeftButton() {
			return Input.GetMouseButtonDown(0);
		}
		public bool Input_KeyDown_Dir(out twin stick) {
			stick = default(twin);
			if (Input.GetKeyDown(KeyCode.RightArrow)) stick.x++;
			if (Input.GetKeyDown(KeyCode.UpArrow)) stick.y++;
			if (Input.GetKeyDown(KeyCode.LeftArrow)) stick.x--;
			if (Input.GetKeyDown(KeyCode.DownArrow)) stick.y--;
			return stick != default(twin);
		}
		public bool Input_KeyDown_PushToggle() { return Input.GetKeyDown(KeyCode.Tab); }
		public bool Input_Key_ShowPalette() { return Input.GetKey(KeyCode.Space); }
		public bool Input_Key_CellValue(out ushort cellValue) {
			KeyCode k = keys.GetCurrentKey();
			if (keyToCellValue.ContainsKey(k)) {
				cellValue = keyToCellValue[k];
				return true;
			} else {
				cellValue = ushort.MaxValue;
				return false;
			}
		}
	}
}