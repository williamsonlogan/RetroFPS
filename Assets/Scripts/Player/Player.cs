using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour {

	// Player Variable
	internal int health = 100, armor = 10; // Internal so that it cannot be modified outside of this assembly
	internal float jumpSpeed = 20.0f;
	internal float sprintMulti = 1.5f;

	// Weapons
	internal WeaponManager weaponMan;
	public Animator gunAnimation = new Animator();
	public LineRenderer shotRenderer = new LineRenderer();
	public AudioSource gunBlast;

	// Player Events
	public event Action OnPlayerDeath;

	// Use this for initialization
	void Start () {
		// Initialize the weapon manager to allow for custom weapon setup and then add weapons accordingly
		weaponMan = new WeaponManager (AvailableWeapons.Custom);

		weaponMan.AddWeapon (Firearms.LaserPistol);
		weaponMan.AddWeapon (Firearms.LaserRifle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	internal void ShootGun(){
		gunBlast.Play ();

		Ray ray = Camera.main.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2));
		RaycastHit hit;

		shotRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));

		if (Physics.Raycast(ray, out hit, 20.0f) && hit.collider.tag == "Enemy") {
			hit.collider.SendMessage("TakeDamage", weaponMan.CurrentWeapon.weaponDamage, SendMessageOptions.DontRequireReceiver);
			Debug.Log ("Enemy Hit");
		}
		if(Physics.Raycast(ray, out hit, 800.0f)){
			shotRenderer.SetPosition(1, hit.point);
		}

		gunAnimation.SetBool ("Shoot", true);
	}

	void TakeDamage(float dmg)
	{
		float difference = 0;
		armor -= Convert.ToInt32 (dmg);

		if (armor < 0) {
			difference = -armor;
			armor = 0;
		}

		Debug.Log ("Player took " + Convert.ToInt32 ((dmg - difference) / 2 + difference) + " damage");
		health -= Convert.ToInt32((dmg - difference) / 2 + difference);

		if (health <= 0) {
			Die ();
		}
	}

	void Die()
	{
		if (OnPlayerDeath != null)
			OnPlayerDeath ();
		//Destroy (gameObject); // We will figure out object destruction later
	}
}
