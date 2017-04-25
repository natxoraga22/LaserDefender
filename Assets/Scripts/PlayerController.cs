﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float playerSpeed = 12f;

	private SpriteRenderer spriteRenderer;
	private float minXPosition;
	private float maxXPosition;


	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		ComputeXPositionLimits ();
	}

	void ComputeXPositionLimits () {
		// Compute the minimun and maximun X position for the player relative to our screen
		float distanceToCamera = this.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(0f, 0f, distanceToCamera));
		Vector3 rightmostPosition = Camera.main.ViewportToWorldPoint (new Vector3(1f, 0f, distanceToCamera));
		minXPosition = leftmostPosition.x + spriteRenderer.bounds.size.x / 2f;
		maxXPosition = rightmostPosition.x - spriteRenderer.bounds.size.x / 2f;
	}
	
	void Update () {
		UpdatePlayerPosition ();
	}

	void UpdatePlayerPosition () {
		// Compute the new X position depending on the player input
		if (Input.GetKey (KeyCode.LeftArrow)) {
			float newXPosition = this.transform.position.x - playerSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			float newXPosition = this.transform.position.x + playerSpeed * Time.deltaTime;
			newXPosition = Mathf.Clamp (newXPosition, minXPosition, maxXPosition);
			this.transform.position = new Vector3 (newXPosition, this.transform.position.y, this.transform.position.z);
		}
	}

}
