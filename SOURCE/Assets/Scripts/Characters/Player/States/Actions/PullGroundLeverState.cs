using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class PullGroundLeverState : SKMecanimState<PlayerCharacterController>
{
	private PullLever_Behaviour pullGroundLeverBehaviour;

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

		if (pullGroundLeverBehaviour == null)
			pullGroundLeverBehaviour = _machine.animator.GetBehaviour<PullLever_Behaviour> ();

		pullGroundLeverBehaviour.onStateExitCallback += OnStateExitPullGroundLever;

		CrossFade ("PullGroundLever", 0.1f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	private void OnStateExitPullGroundLever()
	{
		_machine.changeState<IdleState> ();
	}
}
