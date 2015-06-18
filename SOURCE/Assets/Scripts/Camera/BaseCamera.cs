using UnityEngine;
using System.Collections;

public class BaseCamera : MonoBehaviour 
{
	private Transform cachedTransform;
	public Transform targetPosition;
	public Vector3 positionOffset = new Vector3(0.0f, 1.0f, -20.0f);

	#region PROPERTIES
	public Transform CachedTransform{
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			
			return cachedTransform;
		}
	}

	#endregion

	protected virtual void Awake()
	{
	}
}
