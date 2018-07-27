namespace navdi2 {
	using System.Collections.Generic;

	public class BankEnts<BE> : Ents<BE> where BE:BankableEntity {
		override public void FixedStep() {
			base.FixedStep();
			ClearSleepyEnts();
		}
		
	////INTERNAL functions
		void ClearSleepyEnts() {
			RemoveMatches((ent)=>{return ent.sleepy;});
		}
	}

	public class BankEnts : BankEnts<BankableEntity> {} // special case?
}