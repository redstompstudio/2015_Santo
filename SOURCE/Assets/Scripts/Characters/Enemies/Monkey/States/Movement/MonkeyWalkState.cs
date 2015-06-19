using UnityEngine;
using Prime31.StateKit;

public class MonkeyWalkState : SKMecanimState<PossessedMonkeyController> 
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		CrossFade ("Walk", 0.0f, 0.0f);

		context.visionRange.onTriggerEnterCallback += OnEnterVisionRange;
		context.visionRange.onTriggerExitCallback += OnExitVisionRange;
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		Vector3 targetPosition = context.TargetPosition;

		if(context.TargetTransform != null)
		{
			targetPosition = context.TargetTransform.position;	
		}

		Vector3 direction = (targetPosition - context.Position).normalized;
		direction.y = 0.0f;
		direction.z = 0.0f;

		context.CharacterMotor.Move (direction, context.CharacterSettings.maxRunSpeed * deltaTime);

		if(direction != Vector3.zero)
			context.CharacterMotor.RotateToDirection (direction);

		targetPosition.y = context.Position.y;
		targetPosition.z = context.Position.z;
		float distance = Vector3.Distance (targetPosition, context.Position);

		if(distance < context.stopDistance)
		{
			_machine.changeState<MonkeyMeleeAttackState> ();
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
		Debug.Log ("Idle: Enter Vision Range");

		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null)
		{
			if(context.TargetActor == null)
				context.TargetActor = actor;
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
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (context.Position + Vector3.up * 2, Vector3.one * 0.7f);
	}
#endif
}
