using UnityEngine;
using System.Collections;

public class EnemyPosition : MonoBehaviour {

	void OnDrawGizmos () {
		Gizmos.DrawWireSphere (this.transform.position, 0.25f);
	}

}
