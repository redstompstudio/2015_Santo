using UnityEngine;

[System.Serializable]
public class BaseWeapon : MonoBehaviour
{
	public WEAPON_NAME weaponName;

	public float baseDamage;

	public virtual void Attack()
	{
		
	}
}