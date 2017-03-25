using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public int width = 1280, height = 720;
	public bool full = false;

	public Button settingsButton;
	public Button playButton;
	public Button backButton;

	public GameObject main;
	public GameObject settings;

	// Use this for initialization
	void Start () {
		Screen.SetResolution (width, height, full);
		settings.gameObject.SetActive (false);

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Play () {
		SceneManager.LoadScene ("Demo");
	}

	void Quit () {
		Application.Quit ();
	}

	void OpenSettings()
	{
		settings.gameObject.SetActive (true);
		main.gameObject.SetActive (false);
	}

	void CloseSettings()
	{
		settings.gameObject.SetActive (false);
		main.gameObject.SetActive (true);
	}
}
