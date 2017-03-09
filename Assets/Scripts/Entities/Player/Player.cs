﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : Entity {

	// Player Variable
	internal float jumpSpeed = 20.0f;
	internal float sprintMulti = 1.5f;

	public struct foundKeys{
		public bool red;
		public bool blue;
		public bool yellow;
	};

	public foundKeys keys;

	// Use this for initialization
	void Start () {
		Health = 100;
		Armor = 10;
		Speed = 5.0f;

		WeaponMan = new WeaponManager (AvailableWeapons.Custom);
		WeaponMan.AddWeapon (Firearms.LaserPistol);
		WeaponMan.AddWeapon (Firearms.LaserRifle);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Door") {
			if (Input.GetButton ("Use")) {
				//col.SendMessage ("CheckKey");
				Debug.Log("Hit door");
			}
		}
	}
}
