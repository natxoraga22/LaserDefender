using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	private static int score = 0;

	private Text scoreText;


	void Start () {
		scoreText = GetComponent<Text> ();
		Reset ();
	}

	public static int GetScore () {
		return score;
	}

	public void IncrementScore (int points) {
		score += points;
		UpdateScoreText ();
	}

	public void Reset () {
		score = 0;
		UpdateScoreText ();
	}

	void UpdateScoreText () {
		scoreText.text = "SCORE   " + score;
	}

}
