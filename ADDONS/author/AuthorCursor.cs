namespace navdi2.ADDONS.author {

	using navdi2;
	using UnityEngine;

	public class AuthorCursor : Bod {

		////Setup
		public AuthorCursor Setup(bool fill=false, bool outline=false) {
			base.Setup();
			spriter.enabled=true;
			if (fill) spriter.sprite = fillCursorSprite;
			if (outline) spriter.sprite = outlineCursorSprite;
			return this;
		}
		public AuthorCursor Setup(Vector2 position, bool fill=false, bool outline=false) {
			base.Setup();
			spriter.enabled=true;
			this.position = position;
			if (fill) spriter.sprite = fillCursorSprite;
			if (outline) spriter.sprite = outlineCursorSprite;
			return this;
		}

		////PROPERTIES (inspector)
		public Sprite fillCursorSprite;
		public Sprite outlineCursorSprite;

	}

}