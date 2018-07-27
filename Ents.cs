namespace navdi2 {
	using System.Collections.Generic;

	public class Ents : INavdiEnt {
		public bool active {get;private set;}
		public int maxSize {get;private set;}
		public bool full {get{return maxSize>=0&&ents.Count>=maxSize;}}
		///<description>if an Ents object is 'permanent' it does not empty when Deactivated. its contents are thus 'permanent'.</description>
		public bool permanent {get;private set;}
		protected HashSet<INavdiEnt> ents;
		protected HashSet<INavdiEnt> pendingEnts;
		public Ents(int maxSize = -1, bool permanent = false) {
			this.active = false;
			this.maxSize = maxSize;
			this.permanent = permanent;
			this.ents = new HashSet<INavdiEnt>();
			this.pendingEnts = new HashSet<INavdiEnt>();
		}
		virtual public bool PendingAdd(INavdiEnt ent) {
			if (!this.active&&!this.permanent) { Dj.Error("Can't PendingAdd to inactive Ents "+this); return false; }
			if (maxSize>=0&&ents.Count>=maxSize) { Dj.Error("Can't PendingAdd to full Ents "+this+" (at capacity of "+maxSize+")"); ent.Deactivated(); return false; }
			if (pendingEnts.Add(ent)&&this.active) ent.Activated();
			return true;
		}
		virtual public bool Add(INavdiEnt ent) {
			if (!this.active&&!this.permanent) { Dj.Error("Can't Add to inactive Ents "+this); return false; }
			if (maxSize>=0&&ents.Count>=maxSize) { Dj.Error("Can't Add to full Ents "+this+" (at capacity of "+maxSize+")"); ent.Deactivated(); return false; }
			if (ents.Add(ent)&&this.active) ent.Activated();
			return true;
		}
		virtual public bool Remove(INavdiEnt ent) {
			if (!this.active) { Dj.Error("Can't Remove from inactive Ents "+this); return false; }
			if (this.permanent) { Dj.Error("Can't Remove from permanent Ents "+this); return false; }
			if (ents.Remove(ent)) ent.Deactivated();
			return true;
		}
		virtual public bool Clear() {
			if (this.permanent) { Dj.Error("Can't Clear permanent Ents "+this); return false; }
			DoEach((ent)=>{ent.Deactivated();});
			ents.Clear();
			return true;
		}
		public void DoEach(System.Action<INavdiEnt> eachFunction) {
			foreach(INavdiEnt ent in ents) eachFunction(ent);
		}

	////INavdiEnt
		virtual public void Activated() {
			this.active = true;
			if (permanent) this.DoEach((ent)=>{ent.Activated();}); 
		}
		virtual public void Deactivated() {
			if (permanent) this.DoEach((ent)=>{ent.Deactivated();}); 
			else this.Clear();
			this.active = false;
		}
		virtual public void FrequentStep() { DoEach((ent)=>{ent.FrequentStep();}); }
		virtual public void FixedStep() {
			HashSet<INavdiEnt> sleepyEnts = new HashSet<INavdiEnt>();
			DoEach((ent)=>{
				ent.FixedStep();
				if (ent.IsSleepy()) sleepyEnts.Add(ent); // always be cleaning up sleepy ents
			});
			foreach(INavdiEnt sleepyEnt in sleepyEnts) Remove(sleepyEnt);
			foreach(INavdiEnt pendingAdd in pendingEnts) ents.Add(pendingAdd);
			pendingEnts.Clear();
		}
		virtual public bool IsSleepy() {
			return false; // never sleepy
		}
	}

	public class Ents<E> : Ents where E : INavdiEnt {
		public Ents(int maxSize = -1, bool permanent = false) : base(maxSize,permanent) {}
		sealed override public bool Add(INavdiEnt ent) {
			if (ent is E) return Add((E)ent); else throw Dj.Crashf("INavdiEnt {0} is not of expected type <{1}>", ent, typeof(E).ToString());
		}
		virtual public bool Add(E ent) {
			return base.Add(ent);
			// ents.Add(ent);
		}
		sealed override public bool Remove(INavdiEnt ent) {
			if (ent is E) return Remove((E)ent); else throw Dj.Crashf("INavdiEnt {0} is not of expected type <{1}>", ent, typeof(E).ToString());
		}
		virtual public bool Remove(E ent) {
			return base.Remove(ent);
			// ents.Remove(ent);
		}
		public int RemoveMatches(System.Func<E,bool> matchFunction) {
			HashSet<E> matches = new HashSet<E>();
			DoEach((ent)=>{if(matchFunction(ent))matches.Add(ent);});
			foreach(E match in matches)Remove(match);
			return matches.Count;
		}
		public void DoEach(System.Action<E> eachFunction) {
			foreach(E ent in ents) eachFunction(ent);
		}
	}
}