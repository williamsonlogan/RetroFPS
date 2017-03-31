using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

	// Public class members
	public float range = 10;
	public float fieldOfView = 60.0f; // In degrees
	public float desiredDelay = 5;
	public GameObject player;

	// Private class members
	private bool playerInRange = false;
	private bool playerInSight = false;
	private float laserDelay = 0;
	private Vector3 velocity;
	private Vector3[] path;

	// Testing
	float pathQueryTime;

	void Start () {

		// Add available weapons to enemy - in the future we could randomize this
		WeaponMan = new WeaponManager(AvailableWeapons.None);
		WeaponMan.AddWeapon (Firearms.EnemyLaserPistol);

		// Subscribe the Die function to OnDeath
		OnDeath += Die;

		PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);
	}

	void OnPathFound(Vector3[] newPath, bool success)
	{
		if (success) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		int targetIndex = 0;
		Vector3 currentWaypoint = path [0];

		while (true) {
			if (transform.position == currentWaypoint) {
				if (++targetIndex >= path.Length)
					yield break;
				currentWaypoint = path [targetIndex];
			}

			transform.position = Vector3.MoveTowards (transform.position, currentWaypoint, Speed);
			yield return null;
		}
	}

	void OnDrawGizmos()
	{
		/*Gizmos.color = Color.blue;
		//Gizmos.DrawLine(transform.position, transform.forward * range);

		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, transform.position + DirFromAngle(fieldOfView / 2) * range);
		Gizmos.DrawLine (transform.position, transform.position + DirFromAngle(-fieldOfView / 2) * range);*/
	}

	Vector3 DirFromAngle(float theta)
	{
		theta += transform.eulerAngles.y;
		return new Vector3 (Mathf.Sin (theta * Mathf.Deg2Rad), 0, Mathf.Cos (theta * Mathf.Deg2Rad));;
	}

	void Update () {
		laserDelay--;
		// Check range before sight since the sight has an expensive Linecast call
		playerInRange = PlayerInRange ();
		playerInSight = playerInRange && PlayerInLineOfSight ();

		// If the player is in sight then look directly at it and shoot if possible
		// Otherwise look for the player by rotating
		if (playerInSight) {
			transform.LookAt (player.transform);
			if (WeaponMan.CanFireCurrentWeapon ()) {
				ShootGun (new Ray(transform.position, Vector3.Normalize(player.transform.position - transform.position)));
			}

			// Handles velocity velocity vector (movement)
			if (Vector3.Distance (player.transform.position, transform.position) > 2f)
				velocity = Vector3.Normalize (player.transform.position - transform.position);
			else
				velocity = Vector3.zero;

			velocity.y = 0f;
		} else {
			transform.Rotate (0, 5, 0);
			ShotRenderer.enabled = false;

			velocity = Vector3.zero;
		}

		// laserDelay handles the line renderer timing
		if (laserDelay <= 0.0f) {
			ShotRenderer.enabled = false;
			laserDelay = desiredDelay;
		}

		// Move
		transform.position += velocity * 5 * Time.deltaTime;
	}

	bool PlayerInLineOfSight()
	{
		RaycastHit hit;
		bool isInLineOfSight = false;

		// 1) Check if angle is within FOV, this is done by checking if the angle between the enemy-to-target vector and the enemy-foward vector
		//	  is less than or equal to FOV
		// 2) Next we check to see if a LineCast returns true (this means that the enemy can see something, not necessarily the player)
		// 3) Finally we check to see if the GameObject that was hit was the player
		if (Vector3.Angle (player.transform.position - transform.position, transform.forward) <= (fieldOfView / 2) &&
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
		bool playerIsInRange = false;
		float distance = Vector3.Distance (transform.position, player.transform.position);
			
		if (distance <= range) {
			playerIsInRange = true;
		}

		return playerIsInRange;
	}

	void Die()
	{
		// We can also play a death sound
		Destroy (gameObject);
		Debug.Log ("Enemy Died");
	}
}
