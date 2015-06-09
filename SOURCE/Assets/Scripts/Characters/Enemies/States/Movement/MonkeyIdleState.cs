using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class MonkeyIdleState : SKMecanimState<BaseEnemyController>
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;

		Vector3 velocity = context.CharacterMotor.Velocity;
		velocity.x = 0.0f;

		context.CharacterMotor.SetVelocity (velocity);
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.ResetColliderValues ();

		CrossFade ("Idle", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if(!context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<MonkeyOnAirState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}
}
