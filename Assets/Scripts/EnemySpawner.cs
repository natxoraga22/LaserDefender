using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject blackEnemy;

	public float formationSpeed = 1f;

	private bool isLeftDirection = true;
	private float minXPosition;
	private float maxXPosition;


	void Start () {
		SpawnStartingEnemies ();
		ComputeXPositionLimits ();	// Needs spawned enemies!!
	}

	void ComputeXPositionLimits () {
		// Compute the minimun and maximun X position for the enemy formation relative to our screen
		float distanceToCamera = this.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(0f, 0f, distanceToCamera));
		Vector3 rightmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(1f, 0f, distanceToCamera));
		minXPosition = leftmostPosition.x + GetFormationLeftDistance ();
		maxXPosition = rightmostPosition.x - GetFormationRightDistance ();
	}
		
	// Returns X distance between the lefmost pixel of the formation and the formation's center
	float GetFormationLeftDistance () {
		float formationMinX = this.transform.position.x;
		foreach (Transform enemyPosition in this.transform) {
			Transform enemy = enemyPosition.GetChild (0);
			float enemyMinX = enemy.position.x - enemy.GetComponent<SpriteRenderer> ().bounds.size.x / 2f;
			if (enemyMinX < formationMinX) formationMinX = enemyMinX;
		}
		return Mathf.Abs(this.transform.position.x - formationMinX);
	}

	// Returns X distance between the rightmost pixel of the formation and the formation's center
	float GetFormationRightDistance () {
		float formationMaxX = this.transform.position.x;
		foreach (Transform enemyPosition in this.transform) {
			Transform enemy = enemyPosition.GetChild (0);
			float enemyMaxX = enemy.position.x + enemy.GetComponent<SpriteRenderer> ().bounds.size.x / 2f;
			if (enemyMaxX > formationMaxX) formationMaxX = enemyMaxX;
		}
		return Mathf.Abs(formationMaxX - this.transform.position.x);
	}
 
	void SpawnStartingEnemies () {
		foreach (Transform child in this.transform) {
			Instantiate (blackEnemy, child.position, Quaternion.identity, child);
		}
	}

	void Update () {
		UpdateFormationPosition ();
	}

	void UpdateFormationPosition () {
		// Move the formation to the left until it reaches min X, change direction until reaching max X, change direction again, etc.
		float newXPosition = this.transform.position.x;
		if (isLeftDirection) {
			newXPosition = this.transform.position.x - formationSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			if (newXPosition <= minXPosition) isLeftDirection = false;
		} 
		else {
			newXPosition = this.transform.position.x + formationSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			if (newXPosition >= maxXPosition) isLeftDirection = true;
		}

		this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
	}

}
