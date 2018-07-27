namespace navdi2 {

public class State : Ents {
	public State() : base(permanent:true) {}
	override public void Activated() {
		base.Activated();
	}
	override public void Deactivated() {
		base.Deactivated();
	}
	override public void FrequentStep() {
		base.FrequentStep();
	}
	override public void FixedStep() {
		base.FixedStep();
	}
}

}