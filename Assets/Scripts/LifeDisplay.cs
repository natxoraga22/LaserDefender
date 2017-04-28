using UnityEngine;
using System.Collections;

public class LifeDisplay : MonoBehaviour {

	public void UpdateHealth (int remainingHealth) {
		for (int i = 0; i < this.transform.childCount; i++) {
			this.transform.GetChild (i).gameObject.SetActive (i + 1 <= remainingHealth);
		}
	}

}
