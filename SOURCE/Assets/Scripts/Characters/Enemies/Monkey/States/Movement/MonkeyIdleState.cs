using UnityEngine;
using Prime31.StateKit;

public class MonkeyIdleState : SKMecanimState<PossessedMonkeyController>
{
	private float timeOnIdle;

	public override void begin ()
	{
		base.begin ();

		timeOnIdle = 0.0f;
		_machine.animator.applyRootMotion = false;

		Vector3 velocity = context.CharacterMotor.Velocity;
		velocity.x = 0.0f;

		context.CharacterMotor.SetVelocity (velocity);
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		context.visionRange.onTriggerEnterCallback += OnEnterVisionRange;
		context.visionRange.onTriggerExitCallback += OnExitVisionRange;

		if(context.HasEnemyOnVisionRange)
		{
			context.TargetActor = context.EnemiesOnVision [0];
			_machine.changeState<MonkeyChaseState> ();
			return;
		}

		CrossFade ("Idle", 0.1f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		timeOnIdle += deltaTime;

		if(timeOnIdle >= 3.0f)
		{
			_machine.changeState<MonkeyScoutState> ();
			return;
		}
	}

	public override void end ()
	{
		base.end ();

		context.visionRange.onTriggerEnterCallback -= OnEnterVisionRange;
		context.visionRange.onTriggerExitCallback -= OnExitVisionRange;
	}

	public void OnEnterVisionRange(Collider pOther)
	{
		if (context.TargetActor == null)		//Search for a new target to chase
		{
			BaseActor actor = pOther.GetComponent<BaseActor> ();

			if(actor != null)
			{
				context.TargetActor = actor;
				_machine.changeState<MonkeyChaseState> ();
				return;
			}
		}
	}

	public void OnExitVisionRange(Collider pOther)
	{
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
