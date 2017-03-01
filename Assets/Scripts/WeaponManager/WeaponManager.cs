using System;
using System.Collections.Generic;

public enum WeaponType
{
	Laser, Projectile
};

public class WeaponManager
{
	public float battery = 100.0f;

	// Weapons - Add more weapons as needed/modify the values as needed
	public List<Firearm> Weapons;

	public Firearm CurrentWeapon;

	public WeaponManager()
	{
		Weapons = new List<Firearm>();
		Weapons.Add(new Firearm("Laser Pistol", 1.0f, 0.5f, false, 0.1f, WeaponType.Laser));
		Weapons.Add(new Firearm("Laser Rifle", 5.0f, 2.3f, false, 0.2f, WeaponType.Laser));

		CurrentWeapon = Weapons[0];
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
