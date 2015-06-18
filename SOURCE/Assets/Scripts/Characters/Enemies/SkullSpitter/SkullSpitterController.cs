using UnityEngine;
using System.Collections;

public class SkullSpitterController : MonoBehaviour 
{
	private Transform cachedTransform;
	private Collider cachedCollider;

	private SpawnPool skullPool;
	public string poolName = "SkullProjectile_Pool";

	public Transform spawnPosition;
	public float spitInterval = 2.0f;

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
		InvokeRepeating ("Shoot", 0.0f, spitInterval);
	}

	public void Shoot()
	{
		SkullBulletController skullController = SkullPool.Spawn<SkullBulletController> (spawnPosition.position,
			                                        cachedTransform.rotation);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		RaycastHit hit;
		Physics.Raycast (spawnPosition.position, transform.forward, out hit, 1000.0f);

		Gizmos.DrawLine (spawnPosition.position, hit.point);
	}
	#endif
}
