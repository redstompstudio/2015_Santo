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
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		Vector3 size = context.CharacterMotor.InitialColliderSize;
		size.y = context.CharacterSettings.rollColliderSizeY;
		context.CharacterMotor.ResizeCollider (size);

		if (rollBehaviour == null)
			rollBehaviour = _machine.animator.GetBehaviour<Roll_Behaviour> ();

		rollBehaviour.onStateExitCallback = OnStateExitRoll;

		CrossFade ("Roll", 0.04f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		rollVelocity = context.CharacterMotor.Velocity;
		rollVelocity.x = context.CharacterSettings.rollSpeed * Mathf.Sign(context.Forward.x);

		context.CharacterMotor.SetVelocity (rollVelocity);
	}

	#region STATE_BEHAVIOUR ROLL
	public void OnStateExitRoll()
	{
		if (context.CharacterMotor.IsTouchingCeiling()) 
			_machine.changeState<CrouchState> ();
		else
			_machine.changeState<IdleState> ();
	}
	#endregion
}
