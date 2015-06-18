using UnityEngine;
using System.Collections;

public class BaseAction : MonoBehaviour 
{
	private Transform cachedTransform;

	public bool isExecuting;
	public float actionDelay = 1.5f;

	public Transform CachedTransform{
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return transform;
		}
	}

	protected virtual void Awake()
	{
		
	}

	public virtual void StartAction()	
	{
		isExecuting = true;
	}

	public virtual void StopAction()
	{
		isExecuting = false;
	}
}
