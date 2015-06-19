using UnityEngine;
using System.Collections;

public class ExplosiveBarrel : BaseActor
{
	private SpawnPool explosionFXPool;
	private const string explosionFXPoolName = "Barrel_ExplosionFX_Pool";

	public LayerMask damageLayers;
	public float explosionRadius = 2.0f;

	public int minDamage = 15;
	public int fullDamage = 50;

	#region PROPERTIES
	public SpawnPool ExplosionFXPool{
		get{
			if (explosionFXPool == null)
				explosionFXPool = PoolManager.Instance.GetPool (explosionFXPoolName);

			return explosionFXPool;
		}
	}
	#endregion

	public override void Kill ()
	{
		ExplosionFXPool.Spawn<ParticlePoolObject> (CachedTransform.position, Quaternion.identity);
		CachedGameObject.SetActive (false);

		Collider[] colliders = Physics.OverlapSphere(CachedTransform.position, explosionRadius, damageLayers);

		foreach(Collider col in colliders)
		{
			BaseActor actor = col.GetComponent<BaseActor> ();

			if (actor)
			{
				ApplyDamage (actor, fullDamage, DAMAGE_TYPE.EXPLOSION_SMALL, CachedTransform.position);				
			}
		}

		base.Kill ();
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (CachedTransform.position, explosionRadius);
	}
#endif
}
