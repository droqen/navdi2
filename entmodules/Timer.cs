namespace navdi2.entmodules {
	public class Timer<PhaseKey> : EntModule where PhaseKey : System.IComparable {

		//// constructor, EntModule

		public Timer(PhaseKey defaultPhase, System.Action<PhaseKey> OnPhaseChanged) {
			this.defaultPhase = defaultPhase;
			this.currentPhase = defaultPhase;
			this.OnPhaseChanged = OnPhaseChanged;
		}

		override public void FixedStep() {
			if (this.framesLeft > 0) {
				this.framesLeft--;
				if (this.framesLeft <= 0) {
					this.ChangePhase(this.defaultPhase);
				}
			}
		}

		//// public function calls

		public void ChangePhase(PhaseKey phase, int framesDuration = -1, float secondsDuration = -1) {
			if (this.currentPhase.Equals(phase)) {
				// don't change?
			} else {
				this.currentPhase = phase;
				if (framesDuration > 0) this.framesLeft = framesDuration;
				else if (secondsDuration > 0) this.framesLeft = (int)(secondsDuration / UnityEngine.Time.fixedDeltaTime);
				else this.framesLeft = -1;
				this.OnPhaseChanged(phase);
			}
		}

		//// accessible properties

		public int framesLeft;

		//// private properties

		PhaseKey defaultPhase, currentPhase;
		System.Action<PhaseKey> OnPhaseChanged;
	}
}