using UnityEngine;
using System.Collections;

public class SkullWallController : BaseActor
{
	private SpawnPool destroyFXPool;
	public string destroyFXName = "SkullWall_DestroyFX_Pool";

	#region PROPERTIES
	public SpawnPool DestroyFXPool{
		get{
			if (destroyFXPool == null)
				destroyFXPool = PoolManager.Instance.GetPool (destroyFXName);

			return destroyFXPool;
		}
	}
	#endregion

	protected override void Awake ()
	{
		base.Awake ();

		Health.onDeathCallback += Kill;
	}

	public override void Kill ()
	{
		base.Kill ();

		CachedGameObject.SetActive (false);
		DestroyFXPool.Spawn<ParticlePoolObject> (CachedTransform.position, Quaternion.identity);
	}
}
