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

	public virtual void EquipWeapon(WEAPON_NAME pName)
	{
		UnequipWeapon ();

		equippedWeapon = GetWeapon (pName);
		equippedWeapon.Equip ();
	}

	public virtual void UnequipWeapon()
	{
		if (equippedWeapon != null)
			equippedWeapon.Unequip ();

		equippedWeapon = null;
	}

	public BaseWeapon GetWeapon(WEAPON_NAME pName)
	{
		if (weaponsDictionary.ContainsKey (pName))
			return weaponsDictionary [pName];
		else
			return null;
	}

	#region ANIMATION EVENTS
	public virtual void OnReceivedAttackEvent(string pMessage)
	{
		if (equippedWeapon != null)
			equippedWeapon.OnReceivedAttackEvent(pMessage);
#if UNITY_EDITOR
		else
			Debug.LogError("Received an attack event but there is no equipped weapon!?");
#endif
	}

	public virtual void OnReceivedAttackEvent(int pParam)
	{
		if (equippedWeapon != null)
			equippedWeapon.OnReceivedAttackEvent(pParam);
		#if UNITY_EDITOR
		else
			Debug.LogError("Received an attack event but there is no equipped weapon!?");
		#endif
	}
	#endregion
}
