using UnityEngine;
using System.Collections;

public class BasicRifleWeapon : BaseWeapon
{
	private SpawnPool hitGroundFXPool;
	private const string hitGroundFXName = "Rifle_HitGroundFX_Pool";

	public Transform riflePoint;

	#region PROPERTIES
	public SpawnPool HitGroundFXPool{
		get{
			if (hitGroundFXPool == null)
				hitGroundFXPool = PoolManager.Instance.GetPool (hitGroundFXName);

			return hitGroundFXPool;
		}
	}
	#endregion

	public override void Attack ()
	{
		base.Attack ();

		Vector3 direction = ( GetAimPoint () - riflePoint.position ).normalized;
		direction.z = 0.0f;

		Ray ray = new Ray (riflePoint.position, direction);
		RaycastHit hit;

#if UNITY_EDITOR
		Debug.DrawRay (ray.origin, ray.direction * Range, Color.red, 2.5f);
#endif

		if(Physics.Raycast(ray, out hit, Range))
		{
			HitGroundFXPool.Spawn<ParticlePoolObject>(hit.point, Quaternion.LookRotation(hit.normal));

			BaseActor hitActor = hit.transform.GetComponent<BaseActor> ();

			if(hitActor != null)
			{
				hitActor.Health.DoDamage ( Damage );
			}
		}
	}

	public Vector3 GetAimPoint()
	{
		Vector3 pointerPosition = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay (pointerPosition);
		ray.origin = new Vector3 (ray.origin.x, ray.origin.y, CachedTransform.position.z);

		Vector3 dir = ray.direction * Range;
		dir.z = CachedTransform.position.z;

		return CachedTransform.position + dir;
	}
}
