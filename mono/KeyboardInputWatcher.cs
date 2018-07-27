namespace navdi2.mono
{

	using UnityEngine;
	using System.Collections.Generic;

	public class KeyboardInputWatcher : MonoBehaviour {
	////external
		public string input {get;private set;}
		public void StartWatching(string input, System.Action<string> OnInputChanged, int max = -1) {
			this.input = input;
			this.watching = OnInputChanged != null;
			this.OnInputChanged = OnInputChanged;
			this.max = max;
			ApplyChanges();
		}
		public void StopWatching() {
			StartWatching("", null);
		}
		public void Set(string str) {
			if (watching) {
				this.input = str;
				ApplyChanges();
			}
		}
		public void RegisterAlphabet(string alphabet) {
			this.acceptedInputs = new HashSet<char>();
			foreach(char c in alphabet) this.acceptedInputs.Add(c);
			this.acceptedInputs.Add(' '); // always
		}
		public void ApplyChanges() {
			if (max > 0 && input.Length > max) input = input.Substring(0,max);
			if (OnInputChanged != null) OnInputChanged(input);
		}

	////MonoBehaviour
		void Update() {
			if (watching) {
				bool changed = false;
				foreach(char c in Input.inputString) {
					if (acceptedInputs.Contains(c)) {
						input += c;
						changed = true;
					} else if ((KeyCode)c == KeyCode.Backspace && input.Length > 0) {
						input = input.Substring(0, input.Length-1);
						changed = true;
					}
				}
				if (changed) ApplyChanges();
			}
		}

	////internal
		bool watching;
		int max;
		System.Action<string> OnInputChanged;
		HashSet<char> acceptedInputs;
	}
 
	
}