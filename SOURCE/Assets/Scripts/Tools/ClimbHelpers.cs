using UnityEngine;
using System.Collections;

public class ClimbHelpers
{
	public static Vector3 GetColliderClimbPoint(Vector3 pPosition, Collider pCollider)
	{
		Vector3 searchPoint = pPosition;
		searchPoint.y += pCollider.bounds.size.y;

		return pCollider.ClosestPointOnBounds (searchPoint);
	}
}
