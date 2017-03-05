using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	
	float verticalRotation = 0;
	public float upDownRange = 60.0f;
	float verticalVelocity = 0;

	public Player player;
	
	CharacterController characterController;
	
	// Use this for initialization
	void Start () {
		//Lock Cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		//Create character controller
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}

		UpdateMovement ();
		if (Input.GetButtonDown("Fire1") && player.weaponMan.CanFireCurrentWeapon())
		{
			player.weaponMan.FireCurrentWeapon();
			player.ShootGun();
			player.shotRenderer.enabled = true;
		}
		else
		{
			player.shotRenderer.enabled = false;
			player.gunAnimation.SetBool("Shoot", false);
		}
		UpdateWeapon ();
	}

	void UpdateWeapon()
	{
		int i = player.weaponMan.Weapons.IndexOf (player.weaponMan.CurrentWeapon);

		if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
		{
			if (++i >= player.weaponMan.Weapons.Count)
				i = 0;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
		{
			if (--i < 0)
				i = player.weaponMan.Weapons.Count - 1;
		}

		player.weaponMan.CurrentWeapon = player.weaponMan.Weapons[i];
	}

	void UpdateMovement(){
		// Rotation

		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);


		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


		// Movement

		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

		verticalVelocity += Physics.gravity.y * Time.deltaTime;

		//Code for jumping, didn't feel like it was needed for this game
		//if( characterController.isGrounded && Input.GetButtonDown("Jump") ) {
		//	verticalVelocity = jumpSpeed;
		//}

		Vector3 speed = new Vector3( sideSpeed, verticalVelocity, forwardSpeed );

		speed = transform.rotation * speed;


		//If shift, then sprint(makes this game feel GOOD)
		if(Input.GetButton("Fire3")){
			speed *= player.sprintMulti;
		}

		characterController.Move( speed * Time.deltaTime );

		//If moving, make that gun bob
		if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
			player.gunAnimation.SetBool ("isMoving", false);
		} else {
			player.gunAnimation.SetBool ("isMoving", true);
		}
	}
}
