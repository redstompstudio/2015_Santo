using UnityEngine;
using System.Collections;

public class SkullSpitterController : MonoBehaviour 
{
	private Transform cachedTransform;
	private Collider cachedCollider;

	private SpawnPool skullPool;
	private const string poolName = "SkullProjectile_Pool";

	public Transform spawnPosition;

	#region PROPERTIES
	public Collider CachedCollider{
		get{
			if (cachedCollider == null)
				cachedCollider = GetComponent<Collider> ();
			
			return cachedCollider;
		}
	}

	public SpawnPool SkullPool{
		get{
			if (skullPool == null)
				skullPool = PoolManager.Instance.GetPool(poolName);

			return skullPool;
		}
	}
	#endregion

	void Awake()
	{
		if(cachedTransform == null)
			cachedTransform = GetComponent<Transform> ();
	}

	void Start()
	{
		InvokeRepeating ("Shoot", 0.0f, 1.0f);
	}

	public void Shoot()
	{
		SkullBulletController skullController = SkullPool.Spawn<SkullBulletController> (spawnPosition.position,
			                                        cachedTransform.rotation);
	}
}
