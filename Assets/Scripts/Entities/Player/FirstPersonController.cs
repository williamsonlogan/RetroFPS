using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

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
			//Application.Quit ();
			Debug.Break ();
		}

		UpdateMovement ();
		if (Input.GetButtonDown("Fire1") && player.WeaponMan.CanFireCurrentWeapon())
		{
			player.ShootGun(Camera.main.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2)));
		}
		else
		{
			player.ShotRenderer.enabled = false;
			player.GunAnimation.SetBool("Shoot", false);
		}
		UpdateWeapon ();
	}

	void UpdateWeapon()
	{
		int i = player.WeaponMan.Weapons.IndexOf (player.WeaponMan.CurrentWeapon);

		if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
		{
			if (++i >= player.WeaponMan.Weapons.Count)
				i = 0;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
		{
			if (--i < 0)
				i = player.WeaponMan.Weapons.Count - 1;
		}

		player.WeaponMan.CurrentWeapon = player.WeaponMan.Weapons[i];
	}

	void UpdateMovement(){
		// Rotation

		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);


		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


		// Movement

		float forwardSpeed = Input.GetAxis("Vertical") * player.Speed;
		float sideSpeed = Input.GetAxis("Horizontal") * player.Speed;

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
			player.GunAnimation.SetBool ("isMoving", false);
		} else {
			player.GunAnimation.SetBool ("isMoving", true);
		}
	}
}
