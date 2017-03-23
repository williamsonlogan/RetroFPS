using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pickup : MonoBehaviour {

	public int health; //Added Health
	public int bat; //Added Battery
	public int armor; //Added Armor

	public Firearms.Guns GunDrop;

	//private Firearm realGun;


	// Use this for initialization
	void Start () {
		//realGun = Firearms.PlayerWeapons [(int)GunDrop];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (health > 0)
				col.gameObject.SendMessage ("GetHealth", health);
			if (armor > 0)
				col.gameObject.SendMessage ("GetArmor", armor);
			if (bat > 0)
				col.gameObject.SendMessage ("GetBattery", bat);
			//if (GunDrop != Firearms.Guns.NULLGUN)
				//COMING SOON
				//return;
		}
		Destroy (gameObject);
	}
}
