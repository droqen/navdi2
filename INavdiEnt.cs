namespace navdi2 {
using UnityEngine;

public interface INavdiEnt
{
	void Activated();
	void Deactivated();
	void FrequentStep();
	void FixedStep();

	bool IsSleepy();
}
}