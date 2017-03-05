using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
	
	public float range = 10;
	public float fieldOfView = 60.0f; // In degrees

	bool playerInRange = false;
	bool playerInSight = false;

	public GameObject player;

	void Start () {
		Health = 2;
		Armor = 0;
		Speed = 3.5f;

		// Add available weapons to enemy - in the future we could randomize this
		WeaponMan = new WeaponManager(AvailableWeapons.Custom);
		WeaponMan.AddWeapon (Firearms.EnemyLaserPistol);

		OnDeath += Die;
	}

	void Update () {
		// Check range before sight since the sight has an expensive Linecast call
		playerInRange = PlayerInRange ();
		playerInSight = playerInRange && PlayerInLineOfSight ();

		if (playerInSight) {
			transform.LookAt (player.transform);
			if (WeaponMan.CanFireCurrentWeapon ()) {
				ShootGun (new Ray(transform.position, Vector3.Normalize(player.transform.position - transform.position)));
			}
		} else {
			transform.Rotate (0, 1, 0);
			ShotRenderer.enabled = false;
		}
	}

	bool PlayerInLineOfSight()
	{
		RaycastHit hit;
		bool isInLineOfSight = false;

		// 1) Check if angle is within FOV, this is done by checking if the angle between the enemy-to-target vector and the enemy-foward vector
		//	  is less than or equal to FOV
		// 2) Next we check to see if a LineCast returns true (this means that the enemy can see something, not necessarily the player)
		// 3) Finally we check to see if the GameObject that was hit was the player

		if (Vector3.Angle (player.transform.position - transform.position, transform.forward) <= fieldOfView &&
			Physics.Linecast(transform.position, player.transform.position, out hit) &&
			hit.collider.tag == "Player")
		{
			isInLineOfSight = true;
			//Debug.Log ("Player in line of sight");
		}
			
		return isInLineOfSight;
	}

	bool PlayerInRange()
	{
		// Note that the distance does not call Mathf.Sqrt since that operation is expensive. Instead we compare the squared distance and range
		bool playerIsInRange = false;
		float distSqrd = ((transform.position.x - player.transform.position.x) * (transform.position.x - player.transform.position.x)) +
		                 ((transform.position.y - player.transform.position.y) * (transform.position.y - player.transform.position.y));
			
		if (distSqrd <= range * range) {
			playerIsInRange = true;
		}

		return playerIsInRange;
	}

	void Die()
	{
		Destroy (gameObject);
		Debug.Log ("Enemy Died");
	}
}
