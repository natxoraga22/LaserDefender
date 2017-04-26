using UnityEngine;
using System.Collections;

public class EnemyFormationController : MonoBehaviour {

	public GameObject blackEnemyPrefab;

	public float formationSpeed = 2f;
	public float delayBetweenSpawns = 0.5f;

	private bool isMovingLeft = true;
	private float minXPosition;
	private float maxXPosition;


	void Start () {
		SpawnFullFormation ();
	}

	void SpawnFullFormation () {
		foreach (Transform child in this.transform) {
			Instantiate (blackEnemyPrefab, child.position, Quaternion.identity, child);
		}
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
			if (enemyPosition.childCount > 0) {
				Transform enemy = enemyPosition.GetChild (0);
				float enemyMinX = enemy.position.x - enemy.GetComponent<SpriteRenderer> ().bounds.size.x / 2f;
				if (enemyMinX < formationMinX) formationMinX = enemyMinX;
			}
		}
		return Mathf.Abs(this.transform.position.x - formationMinX);
	}

	// Returns X distance between the rightmost pixel of the formation and the formation's center
	float GetFormationRightDistance () {
		float formationMaxX = this.transform.position.x;
		foreach (Transform enemyPosition in this.transform) {
			if (enemyPosition.childCount > 0) {
				Transform enemy = enemyPosition.GetChild (0);
				float enemyMaxX = enemy.position.x + enemy.GetComponent<SpriteRenderer> ().bounds.size.x / 2f;
				if (enemyMaxX > formationMaxX) formationMaxX = enemyMaxX;
			}
		}
		return Mathf.Abs(formationMaxX - this.transform.position.x);
	}

	void Update () {
		UpdateFormationPosition ();

		if (AllMembersAreDead ()) {
			SpawnUntilFormationIsFull ();
		}
	}

	void UpdateFormationPosition () {
		// Move the formation to the left until it reaches min X, change direction until reaching max X, change direction again, etc.
		float newXPosition = this.transform.position.x;
		if (isMovingLeft) {
			newXPosition = this.transform.position.x - formationSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			if (newXPosition <= minXPosition) isMovingLeft = false;
		} 
		else {
			newXPosition = this.transform.position.x + formationSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			if (newXPosition >= maxXPosition) isMovingLeft = true;
		}

		this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
	}

	bool AllMembersAreDead () {
		foreach (Transform enemyPosition in this.transform) {
			if (enemyPosition.childCount > 0) return false;
		}
		return true;
	}

	void SpawnUntilFormationIsFull () {
		Transform freePosition = GetNextFreePosition ();
		if (freePosition) {
			Instantiate (blackEnemyPrefab, freePosition.position, Quaternion.identity, freePosition);
			ComputeXPositionLimits ();	// Needs spawned enemies!!
		}
		if (GetNextFreePosition ()) {
			Invoke ("SpawnUntilFormationIsFull", delayBetweenSpawns);
		}
	}

	// TODO The next free position should be random!!
	Transform GetNextFreePosition () {
		foreach (Transform enemyPosition in this.transform) {
			if (enemyPosition.childCount == 0) return enemyPosition;
		}
		return null;
	}

}
