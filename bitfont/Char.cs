namespace navdi2.bitfont {
	public class Char : Bod {
		public Font font;
		public void Show(char c) {
			spriter.sprite = font.GetCharSprite(c);
		}
		new public Char Setup() {
			base.Setup();
			if (font==null) throw Dj.Crash("'Char' prototype "+this.gameObject.name+" doesn't have a Font assigned");
			return this;
		}
	}
}