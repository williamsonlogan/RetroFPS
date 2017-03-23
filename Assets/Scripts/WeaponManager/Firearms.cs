using System;
using System.Collections.Generic;

[System.Serializable]
public class Firearms
{
	// Player Weapons
	public static Firearm LaserPistol = new Firearm ("Laser Pistol", 1.0f, 0.5f, false, 0.1f, WeaponType.Laser);
	public static Firearm LaserRifle = new Firearm ("Laser Rifle", 5.0f, 2.3f, false, 0.2f, WeaponType.Laser);

	// Enemy Weapons
	public static Firearm EnemyLaserPistol = new Firearm("Adv. Laser Pistol", 7.0f, 1.0f, false, 0.0f, WeaponType.Laser);

	// All Player Weapons
	public static List<Firearm> PlayerWeapons = new List<Firearm>{ LaserPistol, LaserRifle };

	// All Enemy Weapons
	public static List<Firearm> EnemyWeapons = new List<Firearm>{ EnemyLaserPistol };
}
