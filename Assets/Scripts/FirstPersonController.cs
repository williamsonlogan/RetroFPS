using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
	
	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	public float jumpSpeed = 20.0f;
	public float sprintMulti = 1.5f;
	public Animator gunAnimation = new Animator();

	public LineRenderer lineRenderer = new LineRenderer();

	//Weapon vars
	int currentWeapon = 0;
	bool weaponAuto = false;

	//Weapon damage
	public float pistolDmg = 1.0f;
	public float rifleDmg = 5.0f;
	
	float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	float verticalVelocity = 0;
	
	CharacterController characterController;
	
	// Use this for initialization
	void Start () {
		//Lock Cursor
		//Screen.lockCursor = true; //Depricated
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
		if (Input.GetButtonDown ("Fire1")) {
			shootGun ();
			lineRenderer.enabled = true;
		} else {
			lineRenderer.enabled = false;
		}
	}

	void shootGun(){
		RaycastHit hit;
		Vector3 crosshair = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
		Ray ray = Camera.main.ScreenPointToRay(crosshair);

		lineRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z));

		if (Physics.Raycast(ray, out hit, 20.0f))
		{
			if (hit.collider.tag == ("Enemy"))
			{
				hit.collider.SendMessage("TakeDamage", pistolDmg, SendMessageOptions.DontRequireReceiver);
				Debug.Log("Enemy Hit");
			}
		
			lineRenderer.SetPosition (1, hit.point);
		}

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

		//if( characterController.isGrounded && Input.GetButtonDown("Jump") ) {
		//	verticalVelocity = jumpSpeed;
		//}

		Vector3 speed = new Vector3( sideSpeed, verticalVelocity, forwardSpeed );

		speed = transform.rotation * speed;


		//If shift, then sprint
		if(Input.GetButton("Fire3")){
			speed *= sprintMulti;
		}

		characterController.Move( speed * Time.deltaTime );

		if (Input.GetAxis("Vertical") == 0 & Input.GetAxis("Horizontal") == 0) {
			gunAnimation.SetBool ("isMoving", false);
		} else {
			gunAnimation.SetBool ("isMoving", true);
		}
	}
}
