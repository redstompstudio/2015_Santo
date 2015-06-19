using UnityEngine;
using System.Collections;

public class BasicRifleWeapon : BaseWeapon
{
	private BaseCamera mainCamera;

	private SpawnPool hitGroundFXPool;
	private SpawnPool shootFXPool;

	private const string hitGroundFXName = "Rifle_ImpactFX_Pool";
	private const string shootFXName = "Rifle_ShootFX_Pool";

	#region PROPERTIES
	public SpawnPool HitGroundFXPool{
		get{
			if (hitGroundFXPool == null)
				hitGroundFXPool = PoolManager.Instance.GetPool (hitGroundFXName);

			return hitGroundFXPool;
		}
	}

	public SpawnPool ShootFXPool{
		get{
			if (shootFXPool == null)
				shootFXPool = PoolManager.Instance.GetPool (shootFXName);

			return shootFXPool;
		}
	}
	#endregion

	protected override void Awake()
	{
		mainCamera = SceneManager.Instance.MainCamera;	
	}

	public override void Attack ()
	{
		base.Attack ();

		Vector3 direction = ( GetAimPoint () - weaponPoint.position ).normalized;
		direction.z = 0.0f;

		Ray ray = new Ray (weaponPoint.position, direction);
		RaycastHit hit;

#if UNITY_EDITOR
		Debug.DrawRay (ray.origin, ray.direction * Range, Color.red, 2.5f);
#endif

		ShootFXPool.Spawn<ParticlePoolObject> (weaponPoint.position, weaponPoint.rotation);

		if(Physics.Raycast(ray, out hit, Range))
		{
			HitGroundFXPool.Spawn<ParticlePoolObject>(hit.point, Quaternion.LookRotation(hit.normal));
			BaseActor hitActor = hit.transform.GetComponent<BaseActor> ();

			if(hitActor != null)
			{
				if (weaponOwner != null)
					weaponOwner.ApplyDamage (hitActor, Damage, damageType, hit.point);
				else
					hitActor.ReceiveDamage (null, Damage, damageType, hit.point);
			}
		}
	}

	public Vector3 GetAimPoint()
	{
		Vector3 position = Input.mousePosition;
		position.z = -mainCamera.positionOffset.z;

		return Camera.main.ScreenToWorldPoint(position);
	}
}
