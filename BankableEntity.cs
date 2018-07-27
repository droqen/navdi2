namespace navdi2
{


using UnityEngine;

abstract public class BankableEntity : MonoBehaviour, INavdiEnt {

////Properties
	public Bank bank {get;private set;}
	public bool setup {get;private set;}
	public bool asleep {get;private set;}
	public bool sleepy = false;

////Publics for use by Bank
	public void RegisterBank(Bank bank) {
		this.bank = bank;
		this.setup = false;
	}

	////BankableEntity: Setup, Sleep
	public void Setup() {
		if (setup) Dj.Error("BankableEntity "+gameObject+" was called Setup twice in one lifetime");
		setup = true;
		sleepy = false;
	}
	public void Sleep() {
		this.sleepy = true;
	}

////INavdiEnt
	virtual public void Activated() {
		if (!setup) Dj.Error("BankableEntity "+gameObject+" was Activated without being Setup first");
		asleep = false;
		this.gameObject.SetActive(true);
	}
	virtual public void Deactivated() {
		if (this.bank.BankableSleep(this)) {
			setup = false;
			asleep = true;
			this.gameObject.SetActive(false);
		} else {
			Dj.Temp(name+".Deactivated -- failed!");
		}
	}
	virtual public void FrequentStep() {
		
	}
	virtual public void FixedStep() {
		
	}
	virtual public bool IsSleepy() {
		return this.sleepy;
	}
	
}

}