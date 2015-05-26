using UnityEngine;
using System.Collections;

public class CameraFollow : BaseCamera
{
	public Transform target;
	public Vector3 followOffset = new Vector3(0.0f, 1.0f, -10.0f);

	public float followSpeed = 10.0f;

	protected virtual void LateUpdate()
	{
		CachedTransform.position = Vector3.Lerp (CachedTransform.position, target.position + followOffset,
			followSpeed * Time.deltaTime);
	}
}
