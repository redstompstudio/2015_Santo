using UnityEngine;
using System.Collections;

public class BaseActor : MonoBehaviour 
{
#region CACHED COMPONENTS
	private Transform cachedTransform;
	private GameObject cachedGameObject;
#endregion

	[SerializeField]
	private BaseHealth health;

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

	protected virtual void Reset()
	{
		cachedGameObject.SetActive (true);

		health.currentHealth = health.baseMaxHealth;
	}

	public virtual void Kill()
	{
	}
}
