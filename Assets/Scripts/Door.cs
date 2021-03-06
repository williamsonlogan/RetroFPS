﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public enum keytype {None, Red, Blue, Yellow};

	public keytype Key = keytype.None;

	public Player player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CheckKey() {
		switch (Key) {
		case keytype.None:
			Open ();
			break;
		case keytype.Red:
			if (player.keys.red == true) {
				Open ();
			}
			break;
		case keytype.Blue:
			if (player.keys.blue == true) {
				Open ();
			}
			break;
		case keytype.Yellow:
			if (player.keys.yellow == true) {
				Open ();
			}
			break;
		default:
			Debug.Log ("Wrong Key!");
			break;
		}
	}

	void Open() {
		Debug.Log ("Door Open!");
		Destroy (gameObject);
	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			Debug.Log ("Player Touched Door!");
			if (Input.GetButtonDown ("Use")) {
				Debug.Log ("Door check");
				CheckKey ();
			}
		}
	}
}
