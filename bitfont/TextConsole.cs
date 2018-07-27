namespace navdi2.bitfont {

	using System.Collections.Generic;

	using UnityEngine;

	public class TextConsole : Ents<Text> {
	////public (props, funcs)
		public twin rootPosition {
			get { return _rootPosition; }
			set { _rootPosition = value; Refresh(); }
		}
		public string text {
			get { return _text; }
			set { _text = value; if (active) Refresh(); }
		}
		public TextConsole(Bank charBank, twin rootPosition, int maxLineWidth, int lineHeight, bool valignBottom = false, bool centered = false, int sortingOrder = 0, Color colour = default(Color), string startingText = "") {
			this.charBank = charBank;
			this.maxLineWidth = maxLineWidth;
			this.lineHeight = lineHeight;
			this.valignBottom = true;
			this.centered = centered;
			this.sortingOrder = sortingOrder;
			this.colour = colour;
			// has to happen in this order.
			this.txts = new List<Text>();
			this._rootPosition = rootPosition;
			this._text = startingText;
		}
		override public void Activated() { 
			base.Activated();
			Refresh();
		}
		override public void Deactivated() { 
			base.Deactivated();
			txts.Clear();
		}
	////internal
		Bank charBank;

		int maxLineWidth;
		int lineHeight;

		bool valignBottom;
		bool centered;
		int sortingOrder;
		Color colour;
		List<Text> txts;

		twin _rootPosition;
		string _text;
		void Refresh() {
			List<string> lines = BreakIntoLines(_text, maxLineWidth);
			int lineCount = lines.Count;
			int txtsCount = txts.Count;
			if (lineCount > txtsCount) {
				for (int i = txtsCount; i < lineCount; i++) {
					Text txt = new Text(charBank, rootPosition + (valignBottom?twin.up:twin.down) * lineHeight * i, centered:centered, sortingOrder:sortingOrder, colour:colour);
					Add(txt); txts.Add(txt);
				}
				txtsCount = txts.Count;
			}
			for (int i = 0; i < txtsCount; i++) {
				int lineIndex = valignBottom?txtsCount-1-i:i;
				this.txts[i].text = (lineIndex>=0&&lineIndex<lineCount)?lines[lineIndex]:"";
				this.txts[i].FixedStep();
			}
		}

		public static List<string> BreakIntoLines(string fulltext, int width) {
			List<string> lines = new List<string>();
			int lineStartIndex = 0;
			int wordStartIndex = 0;
			int lastWordEndIndex = 0;
			for (int i = 0; i <= fulltext.Length; i++) {
				char c = i < fulltext.Length ? fulltext[i] : '\n'; // EOF
				switch(c) {
					case ' ':
						int wordLength = i - wordStartIndex;
						if (wordLength > 0) {
							int lineLength = i - lineStartIndex;
							if (lineLength > width) {
								if (lineLength > wordLength) {
									lines.Add(fulltext.Substring(lineStartIndex, lastWordEndIndex - lineStartIndex));
									lineStartIndex = lastWordEndIndex + 1;
								} else {
									lines.Add(fulltext.Substring(lineStartIndex, lineLength));
									lineStartIndex = i + 1;
								}
							}
							lastWordEndIndex = i;
						}
						wordStartIndex = i + 1;
						break;
					case '\n': {
						lines.Add(fulltext.Substring(lineStartIndex, i - lineStartIndex));
						lineStartIndex = i + 1;
					} break;
				}
			}
			return lines;
		}
	}
}