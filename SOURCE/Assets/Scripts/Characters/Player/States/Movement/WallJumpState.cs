using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class WallJumpState : SKMecanimState<PlayerCharacterController> 
{
	private WallJump_Behaviour wallJumpBehaviour;

	public override void begin ()
	{
		base.begin ();

		if (wallJumpBehaviour == null)
			wallJumpBehaviour = _machine.animator.GetBehaviour<WallJump_Behaviour> ();

		wallJumpBehaviour.onStateExitCallback += OnStateExitWallJump;

		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
		_machine.animator.applyRootMotion = false;

		Vector3 velocity = context.CharacterMotor.Velocity;
		velocity = (context.Forward * -6.0f) + (Vector3.up * 12.0f);
		context.CharacterMotor.SetVelocity (velocity);

		CrossFade ("WallJump", 0.03f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		context.CharacterMotor.RotateToVelocityDirection (30.0f);
	}

	public void OnStateExitWallJump()
	{
		_machine.changeState<OnAirState> ();
	}
}
