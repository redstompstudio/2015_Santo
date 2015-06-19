using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseActor : MonoBehaviour 
{
#region CACHED COMPONENTS
	private Transform cachedTransform;
	private GameObject cachedGameObject;
#endregion

	[SerializeField]
	private BaseHealth health;
	public List<DAMAGE_TYPE> canReceiveDamageFrom = new List<DAMAGE_TYPE>();

#region PROPERTIES
	public Transform CachedTransform {
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return cachedTransform;
		}
	}
		
	public GameObject CachedGameObject{
		get{
			if (cachedGameObject == null)
				cachedGameObject = gameObject;
			return cachedGameObject;
		}
	}

	public BaseHealth Health
	{
		get{
			if (health == null)
				health = new BaseHealth ();
			
			return health;
		}
	}

	public Vector3 Position{
		get{return CachedTransform.position;}
	}

	public Quaternion Rotation{
		get{return CachedTransform.rotation;}
	}

	public Vector3 LocalScale{
		get{return CachedTransform.localScale;}
	}

	public Vector3 Forward{
		get{return CachedTransform.forward;}
	}
#endregion

	protected virtual void Awake()	
	{
	}

	protected virtual void Start()
	{
	}

	public virtual void ApplyDamage (BaseActor pTarget, int pDamage, 
									DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		if(pTarget != null)
			pTarget.ReceiveDamage (this, pDamage, pDamageType, pPosition);
	}

	public virtual void ReceiveDamage (BaseActor pCauser, int pDamage, 
										DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		if (pCauser != null)
			pCauser.OnAppliedDamage (this, pDamage, pDamageType, pPosition);

		Health.DoDamage (pDamage);

		if(Health.CurrentHealth <= 0)
		{
			Kill ();
		}
	}

	protected virtual void OnAppliedDamage (BaseActor pTarget, int pDamage, 
	                                       DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		
	}

	public virtual void Reset()
	{
		CachedGameObject.SetActive (true);
		health.currentHealth = health.baseMaxHealth;
	}

	public virtual void Kill()
	{
	}
}
