using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class WallJumpState : SKMecanimState<PlayerCharacterController> 
{
	private WallJump_Behaviour wallJumpBehaviour;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		context.CharacterSettings.enableAirControl = false;

		if (wallJumpBehaviour == null)
			wallJumpBehaviour = _machine.animator.GetBehaviour<WallJump_Behaviour> ();

		wallJumpBehaviour.onStateExitCallback = OnStateExitWallJump;

		context.CharacterMotor.StopMovement (true, true);

		Vector3 velocity = (context.Forward * context.CharacterSettings.wallJumpHorizontalForce) + 
			(Vector3.up * context.CharacterSettings.wallJumpVerticalForce);
		
		velocity.z = 0.0f;

		context.CharacterMotor.SetVelocity (velocity);

		velocity.y = 0.0f;
		context.CharacterMotor.RotateToDirection (velocity);

		CrossFade ("WallJump", 0.03f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public void OnStateExitWallJump()
	{
//		Vector3 velocity = (context.Forward * -6.0f) + (Vector3.up * 12.0f);
//		context.CharacterMotor.SetVelocity (velocity);
//
//		velocity.y = 0.0f;
//		context.CharacterMotor.RotateToDirection (velocity);

		_machine.changeState<OnAirState> ();
	}
}
