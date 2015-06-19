using UnityEngine;
using System.Collections.Generic;

public class BaseEnemy : BaseCharacterController
{
	protected BaseActor targetActor;
	protected Transform targetTransform;
	protected Vector3 targetPosition;

	public TriggerHandler visionRange;
	public float stopDistance = 1.5f;

	//[HideInInspector] 
	public List<BaseActor> enemiesOnVisionRange = new List<BaseActor>();

	#region PROPERTIES
	public BaseActor TargetActor{
		get{
			return targetActor;
		}
		set{
			targetActor = value;

			if(targetActor != null)
			{
				TargetTransform = targetActor.CachedTransform;
				targetPosition = targetTransform.position;
			}
		}
	}

	public Transform TargetTransform{
		get{
			return targetTransform;
		}
		set{
			targetTransform = value;

			if (targetTransform != null)
				targetPosition = targetTransform.position;
		}
	}

	public Vector3 TargetPosition{
		get{
			return targetPosition;
		}
	}

	public List<BaseActor> EnemiesOnVision{
		get{
			return enemiesOnVisionRange;
		}
	}

	public bool HasEnemyOnVisionRange{
		get{
			return (enemiesOnVisionRange != null && enemiesOnVisionRange.Count > 0);
		}
	}
	#endregion

	protected override void Awake()
	{
		base.Awake();

		visionRange.onTriggerEnterCallback += OnEnterVisionRange;
		visionRange.onTriggerExitCallback += OnExitVisionRange;
	}

	#region VISION RANGE
	public void OnEnterVisionRange(Collider pOther)
	{
		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null && !enemiesOnVisionRange.Contains(actor))
		{
			enemiesOnVisionRange.Add (actor);
		}
	}

	public void OnExitVisionRange(Collider pOther)
	{
		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null && enemiesOnVisionRange.Contains(actor))
		{
			enemiesOnVisionRange.Remove (actor);
		}
	}
	#endregion
}
