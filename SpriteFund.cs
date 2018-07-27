namespace navdi2
{
using UnityEngine;
public class SpriteFund : MonoBehaviour {
	[SerializeField] Sprite[] sprs;
	public Sprite this[int index] {
		get { return sprs[index]; }
	}
	public int Length {
		get { return sprs.Length; }
	}
}
}