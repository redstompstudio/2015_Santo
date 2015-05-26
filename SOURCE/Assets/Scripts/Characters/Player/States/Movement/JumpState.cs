using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class JumpState : SKMecanimState<PlayerCharacterController> 
{
	public JumpState()
	{
	}

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		//_machine.animator.SetTrigger ("OnBeginJump");
		CrossFade (context.GetStateTransition ("Jump"));

		context.CharacterMotor.AddVelocity(Vector3.up * context.CharacterMotor.JumpForce);
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

	public override void end ()
	{
		base.end ();
	}
}
