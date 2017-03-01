using UnityEngine;

public class Firearm
{
	// Public Members
	public string weaponName = "";

	public float weaponFireRate = 0.0f;
	public float weaponDamage = 0.0f;
	public float batteryConsumption = 0;

	public WeaponType weaponType = WeaponType.Laser;
	public bool isAuto = false;

	// Private Members
	private float nextFireTime = 0.0f;

	// Constructors
	public Firearm(string _weaponName = "", float _weaponDamage = 0.0f, float _weaponFireRate = 0.0f, bool _isAuto = false,
				   float _batteryConsumption = 0.0f, WeaponType _weaponType = WeaponType.Laser)
	{
		isAuto = _isAuto;
		weaponName = _weaponName;
		weaponType = _weaponType;
		weaponDamage = _weaponDamage;
		weaponFireRate = _weaponFireRate;
		batteryConsumption = _batteryConsumption;
	}

	// Public Member Functions
	public bool CanFire()
	{
		return (Time.time > nextFireTime);
	}

	public void UpdateFireTime()
	{
		nextFireTime = Time.time + weaponFireRate;
	}
}
