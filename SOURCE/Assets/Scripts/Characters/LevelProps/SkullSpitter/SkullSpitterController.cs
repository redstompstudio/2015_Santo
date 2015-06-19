using UnityEngine;
using System.Collections;

public class SkullSpitterController : BaseActor 
{
	private Collider cachedCollider;

	private SpawnPool skullPool;
	public string projectilePoolName = "SkullProjectile_Pool";

	private SpawnPool destroyFXPool;
	public string destroyFXPoolName = "SkullWall_Fire_DestroyFX_Pool";

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
				skullPool = PoolManager.Instance.GetPool(projectilePoolName);

			return skullPool;
		}
	}

	public SpawnPool DestroyFXPool{
		get{
			if (destroyFXPool == null)
				destroyFXPool = PoolManager.Instance.GetPool (destroyFXPoolName);

			return destroyFXPool;
		}	
	}
	#endregion

	void Start()
	{
		StartCoroutine(Shoot());
	}

	public override void ReceiveDamage (BaseActor pCauser, int pDamage, DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		if(canReceiveDamageFrom.Contains(pDamageType))
			base.ReceiveDamage (pCauser, pDamage, pDamageType, pPosition);
	}

	public override void Kill ()
	{
		base.Kill ();

		DestroyFXPool.Spawn<ParticlePoolObject> (CachedTransform.position, Quaternion.identity);
		CachedGameObject.SetActive (false);
	}

	private IEnumerator Shoot()
	{
		while (CachedGameObject.activeInHierarchy) 
		{
			SkullBulletController skullController = SkullPool.Spawn<SkullBulletController> (spawnPosition.position,
				                                        CachedTransform.rotation);
			
			yield return new WaitForSeconds (spitInterval);
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;

		RaycastHit hit;
		Physics.Raycast (spawnPosition.position, transform.forward, out hit, 100.0f);

		Gizmos.DrawLine (spawnPosition.position, hit.point);
	}
#endif
}
