namespace navdi2.mono
{

	using UnityEngine;
	using System.Collections.Generic;

	public class KeyStackWatcher : MonoBehaviour {
		public List<KeyCode> keyStack;
		public List<KeyCode> ignoreKeys;
		void Awake() {
			IgnoreKeys();
		}
		public void IgnoreKeys(params KeyCode[] keys) {
			ignoreKeys = new List<KeyCode>(keys);
			ignoreKeys.Add(KeyCode.None);
			keyStack = new List<KeyCode>();
			keyStack.Add(KeyCode.None);
		}
		void OnGUI() {
			Event e = Event.current;
			if (e.isKey && !ignoreKeys.Contains(e.keyCode)) {
				keyStack.Remove(e.keyCode);
				if (e.type == EventType.keyDown) keyStack.Add(e.keyCode);
			}
		}
		public KeyCode GetCurrentKey() {
			int ct = keyStack.Count;
			return keyStack[ct-1];
		}
	}
 
	
}