using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class JumpState : SKMecanimState<PlayerCharacterController> 
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		context.CharacterMotor.AddVelocity(Vector3.up * context.CharacterSettings.jumpForce);

		CrossFade ("Jump", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if (!context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<OnAirState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}
}
