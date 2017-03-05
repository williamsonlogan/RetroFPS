using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

	Player player;

	public Text healthText;
	public Text armorText;
	public Text weaponText;

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
	}

	public void GameOver()
	{
		Debug.Log ("Player Died");
	}
}
