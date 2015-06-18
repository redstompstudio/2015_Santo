using UnityEngine;
using System.Collections;

public class CameraFollow : BaseCamera
{
	public float followSpeed = 10.0f;

	protected virtual void LateUpdate()
	{
		CachedTransform.position = Vector3.Lerp (CachedTransform.position, targetPosition.position + positionOffset,
			followSpeed * Time.deltaTime);
	}
}
