using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer instance = null;

	public AudioClip startClip;
	public AudioClip gameClip;
	public AudioClip endClip;

	private AudioSource audioSource;


	void Awake () {
		if (instance == null || instance == this) {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		} 
		else {
			GameObject.Destroy (gameObject);
		}

		audioSource = GetComponent<AudioSource> ();
	}

	private void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoaded; // subscribe
	}

	private void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoaded; //unsubscribe
	}

	// This is the new OnLevelWasLoaded method. You may name it as you want
	// Make sure to subscribe/unsubscribe the correct method name (see above)
	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		audioSource.Stop ();
		switch (scene.name) {
		case "Start":
			audioSource.clip = startClip;
			break;
		case "Game":
			audioSource.clip = gameClip;
			break;
		case "Lose":
			audioSource.clip = endClip;
			break;
		}
		audioSource.loop = true;
		audioSource.Play ();
	}

}
