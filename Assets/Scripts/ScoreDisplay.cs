using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

	void Start () {
		Text scoreText = GetComponent<Text> ();
		scoreText.text = "SCORE   " + ScoreKeeper.GetScore ();
	}

}
