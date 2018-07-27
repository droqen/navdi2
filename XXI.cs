namespace navdi2 {

using UnityEngine;
using System.Collections.Generic;

abstract public class XXI : MonoBehaviour {

	//// primary PROPERTIES
	public Dictionary<string, Bank> banks;
	public State state { get; private set; }


	//// PUBLIC
	public void SetState(State state) {
		if (state == null) throw Dj.Crash("XXI.SetState cannot accept a null State parameter");
		if (this.state != null) this.state.Deactivated();
		this.state = state;
		this.state.Activated();
	}
	public Bank GetBank(string bankName) {
		try {
			return banks[bankName];
		} catch (System.Exception e) {
			throw new NoBankException("(GetBank) Navdi has no child '"+bankName+"'\n"+e.Message);
		}
	}
	public class NoBankException : System.Exception {
		public NoBankException(string msg) : base(msg) {}
	}

	
	//// INITIALIZE: protected functions (abstract and virtual, Initialize MUST be overridden)
	abstract protected void Initialize();
	virtual protected bool Initialize_Assert(out string init_error) {
		init_error = "";
		if (state == null) {
			init_error = "Navdi.XXI cannot have a null state. You must set a state during Initialize().";
			return false;
		}
		return true;
	}


	//// MonoBehaviour
	virtual protected void Awake() {
		// make a bank for each child
		this.banks = new Dictionary<string, Bank>();
		// Transform spawn = new GameObject("{Spawn}").transform;
		Transform sleep = new GameObject("{zzz}").transform;
		foreach(Transform child in transform) {
			BankableEntity b = child.GetComponent<BankableEntity>();
			if (b == null) Dj.Error("(Awake) Navdi contains non-bankable child "+child.gameObject.name);
			banks.Add(b.gameObject.name, new Bank(b, new GameObject("{"+b.gameObject.name+"s}").transform, sleep));
			b.gameObject.SetActive(false); // hide the children
		}
		Initialize();
		string init_error;
		if (!Initialize_Assert(out init_error)) {
			throw new System.Exception("navdi2.XXI.Initialize failed: "+init_error);
		}
	}
	virtual protected void Update() {
		this.state.FrequentStep();
	}
	virtual protected void FixedUpdate() {
		this.state.FixedStep();
	}


}

}