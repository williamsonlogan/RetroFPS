using System;
using System.Collections.Generic;

public enum WeaponType
{
	Laser, Projectile
};

public enum AvailableWeapons
{
	AllPlayer, AllEnemy, None
};

public class WeaponManager
{
	public float battery = 100.0f; // Available battery energy - used with each shot
	public List<Firearm> Weapons; // Weapons available to the parent entity
	public Firearm CurrentWeapon; // Weapon currently equipped to the parent entity

	public WeaponManager(AvailableWeapons _availableWeapons = AvailableWeapons.AllPlayer)
	{
		Weapons = new List<Firearm>();

		// Convienent ways to add all player/enemy weapons from the static Firearms class
		switch (_availableWeapons) {
			case AvailableWeapons.AllPlayer:
				foreach (Firearm firearm in Firearms.PlayerWeapons)
					Weapons.Add (firearm);
				CurrentWeapon = Weapons [0];
				break;
			case AvailableWeapons.AllEnemy:
				foreach (Firearm firearm in Firearms.EnemyWeapons)
					Weapons.Add (firearm);
				CurrentWeapon = Weapons [0];
				break;
			case AvailableWeapons.None:
			default:
				CurrentWeapon = null; // Set the CurrentWeapon reference to null for checking in AddWeapon
				break;
		}
	}

	// Adds a weapon to the weapons list and if there is no current weapon reference then we set it to the first item in the weapons list
	public void AddWeapon(Firearm _newFirearm)
	{
		Weapons.Add (_newFirearm);

		if (CurrentWeapon == null) {
			CurrentWeapon = Weapons [0];
		}
	}

	// Updates the battery and the next available fire time
	public void FireCurrentWeapon()
	{
		UpdateBattery();
		CurrentWeapon.UpdateFireTime();
	}

	// Checks is the weapon is cooled down and if there is enough battery left to shoot with
	public bool CanFireCurrentWeapon()
	{
		return (CurrentWeapon.CanFire() && battery > CurrentWeapon.batteryConsumption);
	}

	// Subtracts the current weapons battery consumption from the battery
	public void UpdateBattery()
	{
		battery -= CurrentWeapon.batteryConsumption;
	}
}
