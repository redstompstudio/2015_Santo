using UnityEngine;

[System.Serializable]
public class BaseWeapon : MonoBehaviour
{
	private Transform cachedTransform;

	private bool isEquipped;

	public WEAPON_NAME weaponName;

	public int baseDamage;
	public float baseRange;

	#region PROPERTIES
	public Transform CachedTransform{
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return cachedTransform;
		}
	}

	public bool IsEquipped{
		get{
			return isEquipped;
		}
	}

	public int Damage{
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