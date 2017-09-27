using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static int height = 20;
	public static int width = 10;
	public static int startX = -4;
	public static int startY = 21;
	public Text scoreText;
	public Text highscoreText;
	private int score;
	private int highscore;
	private bool gameOver;
	public Button PlayButton;
	public GameObject PlayAgainButton;
	public Transform spawner;

	void Start() {
		Time.timeScale = 0f;
		score = 0;
		if (!(Application.platform == RuntimePlatform.Android) &&
			!(Application.platform == RuntimePlatform.IPhonePlayer)) {
			foreach (GameObject button in GameObject.FindGameObjectsWithTag("ControlButtons")) {
				button.SetActive (false);
			}
			/*
			// -418, 314
			// -418, 220
			// pause: 297, 252
			Vector3 tempScore = new Vector3(-418f, 314f, 0f);
			GameObject.Find("Score").transform.position = tempScore;
			Vector3 tempHighscore = new Vector3(-418f, 220f, 0f);
			GameObject.Find ("Score").transform.position = tempHighscore;
			Vector3 tempPause = new Vector3(297f, 252f, 0f);
			GameObject.Find("Score").transform.position = tempPause;
			*/
		}
	}

	public void PlayGame() {
		PlayButton.gameObject.SetActive (false);
		PlayAgainButton.SetActive (false);
		ClearGrid ();
		Time.timeScale = 1f;
		score = 0;
		gameOver = false;
		FindObjectOfType<Spawner> ().NextBlock ();
	}

	void FixedUpdate() {
		scoreText.text = "Pontos: " + score;
		if (score > highscore)
			highscore = score;
		highscoreText.text = "Recorde: " + highscore;
		if (gameOver) {
			Time.timeScale = 0f;
			PlayAgainButton.SetActive (true);
		}
	}

	void ClearGrid() {
		foreach (GameObject tetro in GameObject.FindGameObjectsWithTag("Tetromino")) {
			Destroy (tetro);
		}
	}

	public void CheckGameOver(Vector2 position) {
		if (Mathf.RoundToInt (position.x) == Mathf.RoundToInt (spawner.position.x) &&
		    Mathf.RoundToInt (position.y) == Mathf.RoundToInt (spawner.position.y)) {
			gameOver = true;
		}
	}

	public bool InsideGrid(Vector2 position) {
		return ((int)position.x > startX && (int)position.x <= ( startX + width) && (int)position.y >= (startY - height));
	}

	public Vector2 Roudup(Vector2 vector) {
		return new Vector2 (Mathf.Round (vector.x), Mathf.Round (vector.y));
	}

	public void CheckForLines() {
		ArrayList[] lines = new ArrayList[height];
		int currentLine = 0;
		foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
			CubeController cubeControllerInstance = block.GetComponent<CubeController> ();
			if (!cubeControllerInstance.active) {
				currentLine = Mathf.RoundToInt(block.transform.position.y) + height - startY;
				if (currentLine < height) {
					if (lines [currentLine] == null)
						lines [currentLine] = new ArrayList ();
					lines [currentLine].Add (block);
				}
			}
		}

		for (int i = 0; i < height; i++) {
			if (lines[i] != null) {
				if (lines[i].Count == width) {
					score += 50;
					foreach (GameObject block in lines[i].ToArray()) {
						Destroy (block);
					}
					for (int o = i + 1; o < height; o++) { 
						if (lines [o] != null) {
							foreach (GameObject block in lines[o].ToArray()) {
								block.GetComponent<CubeController> ().FallDown ();
							}
						}
					}
				}
			}
		}
	}
	
}
