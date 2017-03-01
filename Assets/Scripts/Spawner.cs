using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	
	public float spawnstart = 3f;
	public float spawndelay = 5f;
	public float radius;
	private float spawnradius;
	private float spawnradius2;
//	public GameObject FirstPersonController;
	public GameObject Enemy;
	private Vector3 spawncircle;

	// Use this for initialization
	void Start () {


		InvokeRepeating ("Spawn", spawnstart, spawndelay);
	}
	
	void Spawn()
	{

		int spawnPointIndex = Random.Range ((int)(radius*100), (int)(radius *-100));
		spawnradius = (float)spawnPointIndex / 100;
		int spawnPointIndex2 = Random.Range ((int)(radius*100), (int)(radius *-100));
		spawnradius2 = (float)spawnPointIndex2 / 100;
		spawncircle = new Vector3( (transform.position.x + spawnradius),transform.position.y , (transform.position.z + spawnradius2) );

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Instantiate (Enemy, spawncircle, transform.rotation);
	}

}
