using UnityEngine;
using System.Collections;

public class BaseCamera : MonoBehaviour 
{
	private Transform cachedTransform;

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
