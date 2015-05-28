using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour 
{
	public BaseWeapon equippedWeapon;

	public List<BaseWeapon> weapons = new List<BaseWeapon>();
	private Dictionary<WEAPON_NAME, BaseWeapon> weaponsDictionary = new Dictionary<WEAPON_NAME, BaseWeapon>();

	protected virtual void Awake()
	{
		foreach(BaseWeapon weapon in weapons)
			weaponsDictionary.Add (weapon.weaponName, weapon);
	}

	public BaseWeapon GetWeapon(WEAPON_NAME pName)
	{
		if (weaponsDictionary.ContainsKey (pName))
			return weaponsDictionary [pName];
		else
			return null;
	}
}
