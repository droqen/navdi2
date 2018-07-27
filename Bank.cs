namespace navdi2
{
	using UnityEngine;
	using System.Collections.Generic;

	public class Bank {
		BankableEntity original;
		Transform spawn;
		Transform sleep;
		List<BankableEntity> spawnedBankables;
		List<BankableEntity> sleepingBankables;
	////Unity
		public Bank(BankableEntity original, Transform spawn, Transform sleep) {
			this.original = original;
			this.spawn = spawn;
			this.sleep = sleep;
			this.spawnedBankables = new List<BankableEntity>();
			this.sleepingBankables = new List<BankableEntity>();
		}
	////Public functions
		public B GetOriginal<B>() where B : BankableEntity {
			if (original.gameObject.GetComponent<B>()==null) throw new System.Exception("Bankable "+original.gameObject.name+" is not of type "+typeof(B));
			return (B)original;
		}
		public B Spawn<B>() where B : BankableEntity {
			B bankable;
			if (sleepingBankables.Count > 0) {
				try {
					bankable = (B)sleepingBankables[0];
				} catch (System.InvalidCastException e) {
					throw Dj.Crashf("Bank '''{0}''' cannot be cast to type '''{1}'''\n{2}",original.name,typeof(B).ToString(),e.Message);
				}
				sleepingBankables.Remove(bankable);
			} else {
				if (original.gameObject.GetComponent<B>()==null) throw new System.Exception("Bankable "+original.gameObject.name+" is not of type "+typeof(B));
				bankable = GameObject.Instantiate(original.gameObject).GetComponent<B>();
				bankable.RegisterBank(this);
			}
			spawnedBankables.Add(bankable);
			bankable.transform.SetParent(spawn);
			return bankable;
		}
		public bool BankableSleep(BankableEntity b) {
			if (spawnedBankables.Contains(b)) {
				spawnedBankables.Remove(b);
				sleepingBankables.Add(b);
				b.transform.SetParent(sleep);
				return true;
			} else {
				return false;
			}
		}
	////Debug
		public int CountAllBankableEntities() {
			return spawnedBankables.Count + sleepingBankables.Count;
		}
	}

}