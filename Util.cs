namespace navdi2 {
using UnityEngine;
public class Util {
	public static void Shuffle<T>(ref T[] a) {
		for (int i = 0; i < a.Length-1; i++) {
			int j = Random.Range(i, a.Length);
			if (i == j) continue;
			T temp = a[i];
			  a[i] = a[j];
			  a[j] = temp;
		}
	}
	public static int Approach(int a, int b, int rate) {
		if (a+rate<b)return a+rate;else if (a-rate>b)return a-rate;else return b;
	}
	public static float Approach(float a, float b, float rate) {
		if (a+rate<b)return a+rate;else if (a-rate>b)return a-rate;else return b;
	}
}
}