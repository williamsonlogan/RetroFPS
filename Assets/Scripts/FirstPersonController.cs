using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
	int health = 100, armor = 0;
	
	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	public float jumpSpeed = 20.0f;
	public float sprintMulti = 1.5f;
	public Animator gunAnimation = new Animator();

	public LineRenderer lineRenderer = new LineRenderer();

	public AudioSource gunBlast;

	public Text healthText;
	public Text armorText;

	//Weapon Manager
	WeaponManager WeaponMan = new WeaponManager();
	
	float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	float verticalVelocity = 0;
	
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
		if (Input.GetButtonDown("Fire1") && WeaponMan.CanFireCurrentWeapon())
		{
			WeaponMan.FireCurrentWeapon(); // Does not do animations yet
			shootGun();
			lineRenderer.enabled = true;
		}
		else
		{
			lineRenderer.enabled = false;
			gunAnimation.SetBool("Shoot", false);
		}

		/* Once we have more weapons and animations this can be the scroll wheel weapon switch logic
		if (Input.GetAxis("Mouse Wheel") > 0.0f)
		{
			int i = WeaponMan.Weapons.IndexOf(WeaponMan.CurrentWeapon);
			if (++i >= WeaponMan.Weapons.Count)
				i = 0;

			WeaponMan.CurrentWeapon = WeaponMan.Weapons[i];
		}
		*/

		healthText.text = health.ToString ();
		armorText.text = armor.ToString ();
	}

	void shootGun(){
		gunBlast.Play ();

		RaycastHit hit;
		Vector3 crosshair = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
		Ray ray = Camera.main.ScreenPointToRay(crosshair);

		lineRenderer.SetPosition (0, new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z));

		if(Physics.Raycast(ray, out hit, 20.0f)){
			if (hit.collider.tag == ("Enemy"))
			{
				hit.collider.SendMessage("TakeDamage", WeaponMan.CurrentWeapon.weaponDamage, SendMessageOptions.DontRequireReceiver);
				Debug.Log("Enemy Hit");
			}
		}
		if(Physics.Raycast(ray, out hit, 800.0f)){
			lineRenderer.SetPosition(1, hit.point);
		}
			gunAnimation.SetBool ("Shoot", true);
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
			speed *= sprintMulti;
		}

		characterController.Move( speed * Time.deltaTime );

		//If moving, make that gun bob
		if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
			gunAnimation.SetBool ("isMoving", false);
		} else {
			gunAnimation.SetBool ("isMoving", true);
		}
	}
}
