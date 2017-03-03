using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float hp = 2;
	public float speed = 2;
	public float range = 10;
	public float fieldOfView = 60.0f; // In degrees

	WeaponManager EnemyWeaponMan = new WeaponManager (AvailableWeapons.Custom);

	public LineRenderer lineRenderer = new LineRenderer();
	public AudioSource gunBlast;

	bool playerInRange = false;
	bool playerInSight = false;

	public GameObject player;
	// Use this for initialization
	void Start () {
		// Add available weapons to enemy - in the future we could randomize this
		EnemyWeaponMan.AddWeapon (Firearms.EnemyLaserPistol);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (playerInRange) {
			transform.LookAt (player.transform);
			RaycastHit hit;

			if(Physics.Raycast(transform.position, player.transform.position, out hit, range))
				{
				//Debug outline of raycast
				lineRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
				lineRenderer.SetPosition (1, hit.point);

					if (hit.collider.tag == "Player") {
						playerInSight = true;
						if (!playerSeen)
							playerSeen = true;
						Debug.Log ("Player in sight!");
					} 
				}
		}

		if (playerSeen && !playerInSight) {
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
			playerInSight = false;
		}

		transform.position = new Vector3 (transform.position.x, 0.8f, transform.position.z);
		*/

		// Check range before sight since the sight has an expensive Linecast call
		playerInRange = PlayerInRange ();
		playerInSight = playerInRange && PlayerInLineOfSight ();

		if (playerInSight) {
			transform.LookAt (player.transform);
			if (EnemyWeaponMan.CanFireCurrentWeapon ()) {
				EnemyWeaponMan.FireCurrentWeapon ();
				ShootAtPlayer ();
			}
		} else {
			transform.Rotate (0, 1, 0);
		}
	}

	void ShootAtPlayer()
	{
		gunBlast.Play ();

		RaycastHit hit;
		Ray ray = new Ray(transform.position, Vector3.Normalize(player.transform.position - transform.position));

		lineRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y, transform.position.z));
		if(Physics.Raycast(ray, out hit, 20.0f)){
			if (hit.collider.tag == ("Player"))
			{
				hit.collider.SendMessage("TakeDamage", EnemyWeaponMan.CurrentWeapon.weaponDamage, SendMessageOptions.DontRequireReceiver);
				Debug.Log("Player Hit");
			}
		}

		if(Physics.Raycast(ray, out hit, 800.0f)){
			lineRenderer.SetPosition(1, hit.point);
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
			Debug.Log ("Player in line of sight");
		}
			
		return isInLineOfSight;
	}

	bool PlayerInRange()
	{
		// Note that the distance does not call Mathf.Sqrt since that operation is expensive. Instead we compare the squared distance and range
		bool playerIsInRange = false;
		float distSqrd = ((transform.position.x - player.transform.position.x) * (transform.position.x - player.transform.position.x)) +
		                   ((transform.position.y - player.transform.position.y) * (transform.position.y - player.transform.position.y));
			
		if (distSqrd <= range * range)
		{
			playerIsInRange = true;
		}

		return playerIsInRange;
	}

	void TakeDamage(float dmg){
		hp -= dmg;

		if (hp <= 0) {
			GameObject.Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<Collider>().tag == "Player") {
			Debug.Log ("Player in range!");
			//playerInRange = true;
		}
	}
}
