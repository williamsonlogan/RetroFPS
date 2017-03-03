using System;
using System.Collections.Generic;

public enum WeaponType
{
	Laser, Projectile
};

public enum AvailableWeapons
{
	AllPlayer, AllEnemy, None, Custom
};

public class WeaponManager
{
	public float battery = 100.0f;

	// Weapons - Add more weapons as needed/modify the values as needed
	public List<Firearm> Weapons;

	public Firearm CurrentWeapon;

	public WeaponManager(AvailableWeapons _availableWeapons = AvailableWeapons.AllPlayer)
	{
		Weapons = new List<Firearm>();

		switch (_availableWeapons) {
		case AvailableWeapons.AllPlayer:
			Weapons.Add (Firearms.LaserPistol);
			Weapons.Add (Firearms.LaserRifle);
			CurrentWeapon = Weapons [0];
			break;
		case AvailableWeapons.AllEnemy:
			Weapons.Add (Firearms.EnemyLaserPistol);
			CurrentWeapon = Weapons [0];
			break;
		case AvailableWeapons.Custom:
		case AvailableWeapons.None:
		default:
			CurrentWeapon = null;
			break;
		}
	}

	public void AddWeapon(Firearm _newFirearm)
	{
		Weapons.Add (_newFirearm);

		if (CurrentWeapon == null) {
			CurrentWeapon = Weapons [0];
		}
	}

	public void FireCurrentWeapon()
	{
		UpdateBattery();
		CurrentWeapon.UpdateFireTime();
	}

	public bool CanFireCurrentWeapon()
	{
		return (CurrentWeapon.CanFire() && battery > CurrentWeapon.batteryConsumption);
	}

	public void UpdateBattery()
	{
		battery -= CurrentWeapon.batteryConsumption;
	}
}
