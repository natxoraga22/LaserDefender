using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject enemyLaserPrefab;

	public int health = 1;
	public int scoreValue = 100;
	public float enemyLaserSpeed = 10f;
	public float enemyLaserFireRate = 0.2f;

	private SpriteRenderer spriteRenderer;

	private ScoreKeeper scoreKeeper;


	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper> ();
	}

	void Update () {
		HandleShooting ();
	}

	void HandleShooting () {
		float probability = Time.deltaTime * enemyLaserFireRate;
		if (Random.value < probability) ShotLaser ();
	}

	void ShotLaser () {
		// Instantiate the laser
		GameObject enemyLaser = (GameObject) Instantiate (enemyLaserPrefab, this.transform.position, Quaternion.identity);
		float yOffset = spriteRenderer.bounds.size.y / 2f + enemyLaser.GetComponent<SpriteRenderer> ().bounds.size.y / 2f;
		enemyLaser.transform.position += new Vector3 (0f, -yOffset, 2f);	// Enemy projectiles z = 2f

		// Set the laser speed
		enemyLaser.GetComponent<Rigidbody2D> ().velocity = new Vector2(0f, -enemyLaserSpeed);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Projectile projectile = collider.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			HandleHit (projectile);
		}
	}

	void HandleHit (Projectile projectile) {
		// Message the projectile
		projectile.Hit ();

		// Handle the hit to know if the enemy dies
		health -= projectile.GetDamage();
		if (health <= 0) {
			scoreKeeper.IncrementScore (scoreValue);
			Destroy (this.gameObject);
		}
	}

}
