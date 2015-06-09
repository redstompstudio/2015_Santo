using UnityEngine;
using System.Collections;

public class BasicRifleWeapon : BaseWeapon
{
	public Transform riflePoint;

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
			var particle = TrashMan.spawn ("Rifle_HitGround_FX", hit.point, Quaternion.LookRotation (hit.normal));	
			TrashMan.despawnAfterDelay (particle, 2.0f);

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
