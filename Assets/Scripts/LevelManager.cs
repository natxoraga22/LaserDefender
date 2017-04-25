using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public void LoadLevel (string levelName) {
		Debug.Log ("LoadLevel: " + levelName);
		SceneManager.LoadScene (levelName);
	}

	public void LoadNextLevel () {
		Debug.Log ("LoadNextLevel");
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame () {
		Debug.Log ("QuitGame");
		Application.Quit ();
	}

}
