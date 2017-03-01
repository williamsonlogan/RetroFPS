using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float hp = 2;
	public float damage = 5;
	public float speed = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TakeDamage(float dmg){
		hp -= dmg;

		if (hp <= 0) {
			GameObject.Destroy (gameObject);
		}
	}
}
