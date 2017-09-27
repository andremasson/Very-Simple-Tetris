using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisController : MonoBehaviour {

	public bool canRotate;
	public bool canRotate360;

	public float fall;
	public bool isMoving;

	public float speed;
	public float timer;
	private GameManager gameManagerObj;

	int horizontal;
	int vertical;

	private Vector2 touchOrigin = -Vector2.one;

	void Start () {
		isMoving = true;
		timer = speed;
		gameManagerObj = FindObjectOfType<GameManager> ();
	}

	void Update () {
		/*
		if (Time.timeScale == 0f)
			isMoving = false;
		else
			isMoving = true;
		*/

		horizontal = 0;
		vertical = 0;

		if (isMoving) {

			horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
			vertical = (int)(Input.GetAxisRaw ("Vertical"));

			/*
			// comandos touch
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];

				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}

				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;

					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;

					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;

					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;

					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			// Fim touch
			*/

			if (Input.GetKeyUp (KeyCode.LeftArrow) || 
				Input.GetKeyUp (KeyCode.RightArrow) ||
				Input.GetKeyUp (KeyCode.DownArrow)) 
			{
				ResetTimer ();
			}

			if (Input.GetKey (KeyCode.RightArrow) || horizontal > 0) {
				MoveRight ();
			} else if (Input.GetKey (KeyCode.LeftArrow) || horizontal < 0) {
				MoveLeft ();
			} else if (Input.GetKey (KeyCode.DownArrow) || vertical < 0) {
				AccelDown ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Space)) {
				RotateIfPossible ();
			}

			if (Time.time - fall >= 1 && !Input.GetKey(KeyCode.DownArrow)) {
				fall = Time.time;
				FallDown ();
			}
		}
	}

	public void ResetTimer() {
		timer = speed;
	}

	public void MoveRight() {
		if (!isMoving)
			return;
		timer += Time.deltaTime;
		if (timer < speed) {
			return;
		}
		timer = 0;
		transform.position += new Vector3 (1f, 0f, 0f);
		if (!IsValidPosition())
			transform.position += new Vector3 (-1f, 0f, 0f);
	}

	public void MoveLeft() {
		if (!isMoving)
			return;
		timer += Time.deltaTime;
		if (timer < speed) {
			return;
		}
		timer = 0;
		transform.position += new Vector3 (-1f, 0f, 0f);
		if (!IsValidPosition ())
			transform.position += new Vector3 (1f, 0f, 0f);
	}

	void FallDown() {
		transform.position += new Vector3 (0f, -1f, 0f);
		if (!IsValidPosition ()) {
			transform.position += new Vector3 (0f, 1f, 0f);
			enabled = false;
			foreach (Transform child in transform) {
				child.GetComponent<CubeController> ().active = false;
			}
			gameManagerObj.CheckForLines ();
			gameManagerObj.CheckGameOver (transform.position);
			FindObjectOfType<Spawner> ().NextBlock ();
		}
	}

	public void AccelDown() {
		if (!isMoving)
			return;
		timer += Time.deltaTime;
		if (timer < speed) {
			return;
		}
		timer = 0;
		FallDown ();
	}

	public void RotateIfPossible() {
		if (!isMoving)
			return;
		if (canRotate) {
			if (!canRotate360) {
				if (transform.rotation.z < 0) {
					transform.Rotate (0f, 0f, 90f);
					if (!IsValidPosition())
						transform.Rotate (0f, 0f, -90f);
				} else {
					transform.Rotate (0f, 0f, -90f);
					if (!IsValidPosition())
						transform.Rotate (0f, 0f, 90f);				
				}
			} else {
				transform.Rotate (new Vector3(0f, 0f, -90f));
				if (!IsValidPosition())
					transform.Rotate (new Vector3(0f, 0f, 90f));
			}
		}
	}

	bool IsValidPosition() {		
		foreach (Transform child in transform) {
			Vector2 position = gameManagerObj.Roudup (child.position);
			if (!gameManagerObj.InsideGrid (position) || IsCollision())
				return false;
		}
		return true;
	}

	bool IsCollision() {
		foreach (GameObject tetro in GameObject.FindGameObjectsWithTag("Tetromino")) {
			if (this.gameObject != tetro) {
				foreach (Transform block in tetro.transform) {
					foreach (Transform child in transform) {
						if (Mathf.RoundToInt(child.position.x) == Mathf.RoundToInt(block.position.x) && Mathf.RoundToInt(child.position.y) == Mathf.RoundToInt(block.position.y))
							return true;
					}
				}
			}
		}
		return false;
	}
}
