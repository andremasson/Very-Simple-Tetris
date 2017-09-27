using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

	public bool markForDeletion;
	public bool active;

	// Use this for initialization
	void Start () {
		markForDeletion = false;
		active = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FallDown() {
		transform.position += new Vector3 (0f, -1f, 0f);
	}
}
