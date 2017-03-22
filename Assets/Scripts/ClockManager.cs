using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClockManager {
	public double mins = 0;
	public float sec = 0;
	public float mSec = 0; //millisecs
	public float totalSecs = 0; //total time passed in game

	public void ClockIncrement (){
		totalSecs = Time.deltaTime;

		//These two if statements increment larger time functions, tried division but it was bugging. Could be improved later

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
