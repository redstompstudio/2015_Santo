using UnityEngine;
using DarkTonic.MasterAudio;

public class DestructableWall : BaseActor
{
	private SpawnPool destroyFXPool;

	public string destroyFXName = "SkullWall_DestroyFX_Pool";
	public string destroySoundName = "BreakableWall_Crashing";

	#region PROPERTIES
	public SpawnPool DestroyFXPool{
		get{
			if (destroyFXPool == null)
				destroyFXPool = PoolManager.Instance.GetPool (destroyFXName);

			return destroyFXPool;
		}
	}
	#endregion

	public override void ReceiveDamage (BaseActor pCauser, int pDamage, DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		if (canReceiveDamageFrom == null || canReceiveDamageFrom.Count == 0 || 
			canReceiveDamageFrom.Contains (pDamageType))
			base.ReceiveDamage (pCauser, pDamage, pDamageType, pPosition);
	}

	public override void Kill ()
	{
		base.Kill ();

		MasterAudio.PlaySound (destroySoundName);

		CachedGameObject.SetActive (false);
		DestroyFXPool.Spawn<ParticlePoolObject> (CachedTransform.position, Quaternion.identity);
	}	
}
