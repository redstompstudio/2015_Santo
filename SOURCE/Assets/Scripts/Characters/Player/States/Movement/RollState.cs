using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class RollState : SKMecanimState<PlayerCharacterController> 
{
	private Roll_Behaviour rollBehaviour;
	private Vector3 rollVelocity; 

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		if (rollBehaviour == null)
			rollBehaviour = _machine.animator.GetBehaviour<Roll_Behaviour> ();

		rollBehaviour.onStateExitCallback += OnStateExitRoll;

		rollVelocity = context.CharacterMotor.GetRollVelocity();

		CrossFade ("Roll", 0.04f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		rollVelocity.y = context.CharacterMotor.Velocity.y;
		context.CharacterMotor.SetVelocity (rollVelocity);
	}

	#region STATE_BEHAVIOUR ROLL
	public void OnStateExitRoll()
	{
		_machine.changeState<IdleState> ();
	}
	#endregion
}
