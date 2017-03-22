﻿using System;
using UnityEngine;

//Living things class

public class Entity : MonoBehaviour
{
	public int Health = 0;
	public int Armor = 0;
	public float Speed = 0.0f;

	internal WeaponManager WeaponMan;
	public Animator GunAnimation = new Animator();
	public LineRenderer ShotRenderer = new LineRenderer();
	public AudioSource GunBlast;

	public event Action OnDeath;

	void Start()
	{
	}

	public void TakeDamage(int dmg)
	{
		//Armor reduction
		float difference = 0;
		Armor -= Convert.ToInt32 (dmg);

		if (Armor < 0) {
			difference = -Armor;
			Armor = 0;
		}
			
		Debug.Log ("Player took " + Convert.ToInt32 ((dmg - difference) / 2 + difference) + " damage");
		Health -= Convert.ToInt32((dmg - difference) / 2 + difference);

		if (Health <= 0) {
			OnDeath ();
			Health = 0; //Keep health from going negative
		}
	}

	//General gun shooting code
	internal void ShootGun(Ray ray){
		WeaponMan.FireCurrentWeapon ();
		GunBlast.Play ();

		RaycastHit hit;
		if (gameObject.tag == "Player")
			ShotRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
		else
			ShotRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y, transform.position.z));

		if (Physics.Raycast(ray, out hit, 20.0f) && (hit.collider.tag == "Enemy" || hit.collider.tag == "Player")) {
			hit.collider.SendMessage("TakeDamage", WeaponMan.CurrentWeapon.weaponDamage, SendMessageOptions.DontRequireReceiver);
			Debug.Log (hit.collider.tag + " Hit");
		}
		if(Physics.Raycast(ray, out hit, 800.0f)){
			ShotRenderer.SetPosition(1, hit.point);
		}

		if (GunAnimation != null)
			GunAnimation.SetBool ("Shoot", true);

		ShotRenderer.enabled = true;
	}
}