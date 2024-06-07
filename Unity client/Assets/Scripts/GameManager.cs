using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[SerializeField]
	private TextMeshProUGUI player1ScoreLabel;
	[SerializeField]
	private TextMeshProUGUI player2ScoreLabel;
	public static int PlayerScore1 = 0;
	public static int PlayerScore2 = 0;

	public GUISkin layout;

	GameObject theBall;
	[SerializeField]
	private TextMeshProUGUI winnerText;

	// Use this for initialization
	void Start () {
		theBall = GameObject.FindGameObjectWithTag ("Ball");
	}

	public  void Score(string wallID) {
		if (wallID == "RightWall") {
			PlayerScore1++;
		} else {
			PlayerScore2++;
		}
		UpdateScoresLabels();
	}

	private void UpdateScoresLabels()
	{
		player1ScoreLabel.text = PlayerScore1.ToString();
		player2ScoreLabel.text = PlayerScore2.ToString();
	}

	private void Restart()
	{
        PlayerScore1 = 0;
        PlayerScore2 = 0;
        theBall.SendMessage("RestartGame", 0.5f, SendMessageOptions.RequireReceiver);
        winnerText.gameObject.SetActive(false);
    }

    void Update() {


		if (PlayerScore1 == 10) {
			winnerText.gameObject.SetActive(true);
			winnerText.text = "PLAYER ONE WINS";
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		} else if (PlayerScore2 == 10) {
            winnerText.gameObject.SetActive(true);
            winnerText.text = "PLAYER TWO WINS";
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		}
	}

}
