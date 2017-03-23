using UnityEngine;
using UnityEngine.UI;
using System;

public class GameHUD : MonoBehaviour {

	Player player;

	public Text healthText;
	public Text armorText;
	public Text weaponText;
	public Text clockText;
	public Text batteryText;

	ClockManager cMan = new ClockManager();

	int leveCount;

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		player.OnDeath += GameOver; // Subscribing the OnPlayerDeath event to the GameOver function
	}
	
	// Update is called once per frame
	void Update () {
		healthText.text = player.Health.ToString ();
		armorText.text = player.Armor.ToString ();
		weaponText.text = player.WeaponMan.CurrentWeapon.weaponName;
		batteryText.text = "Battery: " + Mathf.RoundToInt (player.WeaponMan.battery * 10) / 10f;

		cMan.ClockIncrement ();
		clockText.text = string.Format("{0}:{1}:{2}", Math.Floor (cMan.mins), Math.Round(cMan.sec, 0), Math.Round(cMan.mSec/100, 0));
	}

	public void GameOver()
	{
		Debug.Log ("Player Died");
	}
}
