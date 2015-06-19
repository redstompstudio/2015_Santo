﻿using UnityEngine;
using Prime31.StateKit;

public class MonkeyIdleState : SKMecanimState<PossessedMonkeyController>
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;

		Vector3 velocity = context.CharacterMotor.Velocity;
		velocity.x = 0.0f;

		context.CharacterMotor.SetVelocity (velocity);
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		if(context.HasEnemyOnVisionRange)
		{
			if(context.TargetActor == null)
				context.TargetActor = context.EnemiesOnVision [0];

			_machine.changeState<MonkeyWalkState> ();
			return;
		}
		else
			context.TargetActor = null;	

		CrossFade ("Idle", 0.1f, 0.0f);
		context.visionRange.onTriggerEnterCallback += OnEnterVisionRange;
		context.visionRange.onTriggerExitCallback += OnExitVisionRange;
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public override void end ()
	{
		base.end ();

		context.visionRange.onTriggerEnterCallback -= OnEnterVisionRange;
		context.visionRange.onTriggerExitCallback -= OnExitVisionRange;
	}

	public void OnEnterVisionRange(Collider pOther)
	{
		Debug.Log ("Idle: Enter Vision Range");

		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null)
		{
			if (context.TargetActor == null) 
			{
				context.TargetActor = actor;
				_machine.changeState<MonkeyWalkState> ();
				return;
			}
		}
	}

	public void OnExitVisionRange(Collider pOther)
	{
		Debug.Log ("Idle: Exit Vision Range");

		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null)
		{
			if(context.TargetActor == actor)
				context.TargetActor = null;
		}
	}

#if UNITY_EDITOR
	public override void OnGizmos ()
	{
		base.OnGizmos ();
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube (context.Position + Vector3.up * 2, Vector3.one * 0.7f);
	}
#endif
}
