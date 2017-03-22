using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	// Public class member
	public float spawnstart = 3f;
	public float spawndelay = 5f;
	public float radius;
	public GameObject target;
	public GameObject prefab;

	// Private class member
	private float _spawnRadiusXOffset;
	private float _spawnRadiusYOffset;
	private Vector3 _spawnCircle;

	// Use this for initialization
	void Start () {

		// Start the repeating function invokation
		InvokeRepeating ("Spawn", spawnstart, spawndelay);
	}
	
	void Spawn()
	{

		// Calculate random X-Offset
		int spawnPointIndex = Random.Range ((int)(radius*100), (int)(radius *-100));
		_spawnRadiusXOffset = (float)spawnPointIndex / 100;

		// Calculate random Y-Offset
		int spawnPointIndex2 = Random.Range ((int)(radius*100), (int)(radius *-100));
		_spawnRadiusYOffset = (float)spawnPointIndex2 / 100;

		// Create the spawn position
		_spawnCircle = new Vector3( (transform.position.x + _spawnRadiusXOffset),transform.position.y , (transform.position.z + _spawnRadiusYOffset) );

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
		Enemy enemy = Instantiate (prefab, _spawnCircle, transform.rotation).GetComponent<Enemy>();

		// Set the enemy's player GameObject to the provided target
		enemy.player = target;
	}
}
