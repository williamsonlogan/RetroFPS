using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float hp = 2;
	public float damage = 5;
	public float speed = 2;
	public float range = 10;

	bool playerInRange = false;
	bool playerInSight = false;
	bool playerSeen = false;

	public LineRenderer lineRenderer = new LineRenderer();

	public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
			playerInRange = true;
		}
	}
}
