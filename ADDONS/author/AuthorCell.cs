namespace navdi2.ADDONS.author {

using navdi2.grid;

abstract public class AuthorCell : GridCell {
	//// AuthorCell predefined publics
	abstract public void SetToPlayMode(State state); // SEALED by AuthorCell<PLAY>
	public byte superType {get{return GetCellSuperType(value);}}
	public byte GetCellSuperType(ushort cellValue) {
		return (byte)(GetCellType(cellValue) & 0xF0);
	}
	public byte type {get{return GetCellType(value);}}

	//// AuthorCell abstract function #1 - MUST OVERRIDE
	abstract public byte GetCellType(ushort cellValue); // returns any AuthorCell_Type until you have something better
}
abstract public class AuthorCell<PLAY> : AuthorCell where PLAY : AuthorState_Play {

	//// PROPERTIES (INSPECTOR)
	public SpriteFund cellValuesSpriteFund;
	virtual public void ShowCellValue(ushort cellValue) {
		// visually represent a particular cellValue
		if (type == (byte)AuthorCell_SuperType.Unused) {
			spriter.enabled = false; // unused!!!
		} else if (cellValue < cellValuesSpriteFund.Length) {
			spriter.enabled = true;
			spriter.sprite = cellValuesSpriteFund[cellValue];
		} else {
			spriter.enabled = false;
			Dj.Error("AuthorCell "+this.name+" cannot show cellValue "+cellValue);
		}
	}

	//// AuthorCell abstract function #2 - MUST OVERRIDE
	abstract public void Spawn(PLAY state, twin cellPos, twin pos, ushort cellValue, out ushort cellErasedValue);

	//// GridCell override
	override public void RefreshValue() {
		switch((AuthorCell_SuperType)GetCellSuperType(value)) {
			case AuthorCell_SuperType.Tile_Collide:
			case AuthorCell_SuperType.Tile_NoCollide:
			case AuthorCell_SuperType.Spawn:
				spriter.enabled = true;
				ShowCellValue(value);
				break;
			case AuthorCell_SuperType.Unused: default:
				spriter.enabled = false;
				break;
		}
	}

	//// AuthorCell PUBLIC set functions
	sealed override public void SetToPlayMode(State state) { SetToPlayMode((PLAY)state); } // be rly rly careful, don't create any infinite loops
	virtual public void SetToPlayMode(PLAY state) {
		switch((AuthorCell_SuperType)GetCellSuperType(value)) {
			case AuthorCell_SuperType.Tile_Collide:
				collides = true;
				break;
			case AuthorCell_SuperType.Tile_NoCollide:
				collides = false;
				break;
			case AuthorCell_SuperType.Spawn:
				collides = false;
				ushort tileErasedValue;
				Spawn(state, cellPos, new twin(roundPosition), value, out tileErasedValue);
				SetValue(tileErasedValue);
				break;
			default:
				collides = false;
				Dj.Error("AuthorCell.SetToPlayMode got invalid supertype "+(AuthorCell_SuperType)GetCellSuperType(value)+" @ value "+value);
				break;
		}
	}
	public void SetValueSafe(ushort value) {
		if (this.value == value) return;
		base.SetValue(value);
	}
}

}