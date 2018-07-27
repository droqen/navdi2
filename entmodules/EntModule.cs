namespace navdi2.entmodules {
	abstract public class EntModule : INavdiEnt {

		virtual public void Activated() {

		}
		virtual public void Deactivated() {

		}
		virtual public void FixedStep() {

		}
		virtual public void FrequentStep() {

		}
		virtual public bool IsSleepy() {
			return false; // never sleepy, for placing at the root of a Navdi.State -- which is always permanent:true
		}

	}
}