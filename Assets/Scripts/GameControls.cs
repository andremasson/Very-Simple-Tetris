using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour {

	[HideInInspector]
	public Transform controller;
	public GameObject PauseButton;
	public GameObject ResumeButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MoveRight() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.MoveRight ();
		instance.ResetTimer ();
	}

	public void MoveLeft() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.MoveLeft ();
		instance.ResetTimer ();
	}

	public void MoveDown() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.AccelDown ();
		instance.ResetTimer ();
	}

	public void Turn() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.RotateIfPossible ();
		instance.ResetTimer ();
	}

	public void Pause() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.isMoving = false;
		Time.timeScale = 0f;
		PauseButton.SetActive (false);
		ResumeButton.SetActive (true);
	}

	public void Resume() {
		TetrisController instance = (TetrisController)controller.GetComponent (typeof(TetrisController));
		instance.isMoving = true;
		Time.timeScale = 1f;
		PauseButton.SetActive (true);
		ResumeButton.SetActive (false);
	}
}
