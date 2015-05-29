using UnityEngine;
using System.Collections;

public class BasicRifleWeapon : BaseWeapon
{
	public Transform riflePoint;

	public override void Attack ()
	{
		base.Attack ();

		Vector3 direction = riflePoint.forward;
		direction.z = 0.0f;

		Ray ray = new Ray (riflePoint.position, direction);
		RaycastHit hit;

		Debug.DrawRay (ray.origin, ray.direction * Range, Color.red, 2.5f);

		if(Physics.Raycast(ray, out hit, Range))
		{
			var particle = TrashMan.spawn ("Rifle_HitGround_FX", hit.point, Quaternion.LookRotation (hit.normal));	
			TrashMan.despawnAfterDelay (particle, 2.0f);
		}
	}
}
