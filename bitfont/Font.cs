namespace navdi2.bitfont {
	using UnityEngine;
	public class Font : SpriteFund {
	////PROPERTIES
		public string alphabet;
		public int charWidth;
		public int lineHeight;
	////CHAR ACCESS?
		public Sprite GetCharSprite(char c) {
			int index = alphabet.IndexOf(c);
			if (index >= 0) {
				if (index < this.Length) {
					return this[index];
				} else {
					Dj.Error("Font "+name+" index >= Length. alphabet/sprs mismatch?");
				}
			}
			return null;
		}
	}
}