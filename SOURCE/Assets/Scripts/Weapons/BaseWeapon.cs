using UnityEngine;

[System.Serializable]
public class BaseWeapon : MonoBehaviour
{
	private bool isEquipped;

	public WEAPON_NAME weaponName;

	public float baseDamage;
	public float baseRange;

	#region PROPERTIES
	public bool IsEquipped{
		get{
			return isEquipped;
		}
	}

	public float Damage{
		get{
			return baseDamage;
		}
	}

	public float Range{
		get{
			return baseRange;
		}
	}
	#endregion

	public virtual void Equip()
	{
		isEquipped = true;
	}

	public virtual void Unequip()
	{
		isEquipped = false;
	}

	public virtual void Attack()
	{
	}

	public virtual void OnReceivedAttackEvent()
	{
	}

	public virtual void OnReceivedAttackEvent(float pParam)
	{
	}

	public virtual void OnReceivedAttackEvent(int pParam)
	{
	}

	public virtual void OnReceivedAttackEvent(Object pParam)
	{
	}

	public virtual void OnReceivedAttackEvent(string pMessage)
	{
	}
}