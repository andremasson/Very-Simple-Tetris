using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Transform[] spawnList;
	public GameObject gameControls;

	void Start () {
		
	}

	public void NextBlock() {
		Transform newObject = Instantiate(spawnList[Random.Range(0, spawnList.Length)], transform.position, Quaternion.identity);
		FindObjectOfType<GameControls> ().controller = newObject;
	}

	void Update () {
		
	}
}
