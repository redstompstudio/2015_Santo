using UnityEngine;
using System.Collections;

public class Raycaster
{
	public class RaycastHitInfo
	{
		public bool hitSomething;
		public RaycastHit hit;

		public RaycastHitInfo(bool pHitted, RaycastHit pHit)
		{
			hitSomething = pHitted;
			hit = pHit;
		}
	}

	public static bool HitSomething(Vector3 pOrigin, Vector3 pDirection, float pDistance, LayerMask pMask)
	{
		Ray ray = new Ray (pOrigin, pDirection);
		RaycastHit hit;

		return Physics.Raycast (ray, out hit, pDistance, pMask.value);
	}

	public static RaycastHitInfo GetRaycastHitInfo(Vector3 pOrigin, Vector3 pDirection, float pDistance, LayerMask pMask)
	{
		Ray ray = new Ray (pOrigin, pDirection);

		RaycastHitInfo hitInfo = new RaycastHitInfo (false, new RaycastHit());
		RaycastHit hit;

		hitInfo.hitSomething = Physics.Raycast (ray, out hit, pDistance, pMask.value);
		hitInfo.hit = hit;
		return hitInfo;
	}

	public static Vector3 GetRayHitPoint(Vector3 pOrigin, Vector3 pDirection, float pDistance)
	{
		Ray ray = new Ray (pOrigin, pDirection);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, pDistance))
		{
			return hit.point;
		}

		return Vector3.zero;
	}
}
