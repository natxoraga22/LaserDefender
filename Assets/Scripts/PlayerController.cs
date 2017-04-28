using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject playerLaserPrefab;

	public int maxHealth = 3;
	private int health;	// = maxHealth
	public float playerSpeed = 12f;
	public float playerLaserSpeed = 20f;
	public float playerLaserFireRate = 0.2f;

	public AudioClip laserSound;

	private LevelManager levelManager;
	private LifeDisplay lifeDisplay;

	private SpriteRenderer spriteRenderer;
	private float minXPosition;
	private float maxXPosition;


	void Start () {
		health = maxHealth;
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		lifeDisplay = GameObject.FindObjectOfType<LifeDisplay> ();
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
		HandleShooting ();
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

	void HandleShooting () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("ShotLaser", 0f, playerLaserFireRate);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke ("ShotLaser");
		}
	}

	void ShotLaser () {
		// Instantiate the laser
		GameObject playerLaser = (GameObject) Instantiate (playerLaserPrefab, this.transform.position, Quaternion.identity);
		float yOffset = spriteRenderer.bounds.size.y / 2f + playerLaser.GetComponent<SpriteRenderer> ().bounds.size.y / 2f;
		playerLaser.transform.position += new Vector3 (0f, yOffset, 0f);

		// Set the laser speed
		playerLaser.GetComponent<Rigidbody2D> ().velocity = new Vector2(0f, playerLaserSpeed);

		// Play laser sound
		AudioSource.PlayClipAtPoint (laserSound, transform.position);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		// Projectile?
		Projectile projectile = collider.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			HandleHit (projectile);
		}

		// Player life?
		if (collider.gameObject.CompareTag ("PlayerLife")) {
			if (health < maxHealth) health++;
			lifeDisplay.UpdateHealth (health);
			Destroy (collider.gameObject);
		}
	}

	void HandleHit (Projectile projectile) {
		health -= projectile.GetDamage();

		// Message the projectile and the UI
		projectile.Hit ();
		lifeDisplay.UpdateHealth (health);

		// Handle the hit to know if we die
		if (health <= 0) {
			Die ();
		}
	}

	void Die () {
		levelManager.LoadLevel ("Lose");
	}

}
