using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float playerSpeed = 12f;

	private SpriteRenderer spriteRenderer;
	private float minX;
	private float maxX;


	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		ComputeXPositionLimits ();
	}

	void ComputeXPositionLimits () {
		// Compute the minimun and maximun X position relative to our screen
		float distance = this.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(0f, 0f, distance));
		Vector3 rightmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(1f, 0f, distance));
		minX = leftmostPosition.x + spriteRenderer.bounds.size.x / 2f;
		maxX = rightmostPosition.x - spriteRenderer.bounds.size.x / 2f;
	}
	
	void Update () {
		UpdatePlayerPosition ();
	}

	void UpdatePlayerPosition () {
		// Compute the new X position
		if (Input.GetKey (KeyCode.LeftArrow)) {
			float newXPosition = this.transform.position.x - playerSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minX, maxX);
			this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			float newXPosition = this.transform.position.x + playerSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minX, maxX);
			this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
		}
	}

}
