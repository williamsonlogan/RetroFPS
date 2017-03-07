using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClockManager : MonoBehaviour {
	public double mins = 0;
	public float sec = 0;
	public float mSec = 0;
	public float totalSecs = 0;

	// Use this for initialization
	void Start () {
		
	}

	public void ClockIncrement (){
		totalSecs = Time.deltaTime;
		if (sec >= 60) {
			sec = 0;
			mins += 1;
		} else {
			sec += Time.deltaTime;
		}
		if (mSec >= 1000) {
			mSec = 0;
		} else {
			mSec += Time.deltaTime * 1000;
		}
	}
}
