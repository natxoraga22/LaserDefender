using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	private int score = 0;

	private Text scoreText;


	void Start () {
		scoreText = GetComponent<Text> ();
		Reset ();
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
