using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public GameObject projectileHitPrefab;

	public int damage = 1;
	public float explosionDuration = 0.1f;
	public bool isShotUpwards = true;

	private SpriteRenderer spriteRenderer;


	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public int GetDamage () {
		return damage;
	}

	public void Hit () {
		// Create an explosion
		Vector3 projectileHitPosition = this.transform.position + new Vector3 (0f, (isShotUpwards ? 1f : -1f) * spriteRenderer.bounds.size.y / 2f, 0f);
		GameObject projectileHit = (GameObject) Instantiate (projectileHitPrefab, projectileHitPosition, Quaternion.identity);
		Destroy (projectileHit, explosionDuration);
		// Destroy the projectile
		Destroy (this.gameObject);
	}

}
