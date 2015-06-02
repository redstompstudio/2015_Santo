using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class LandState : SKMecanimState<PlayerCharacterController>
{
	public Land_Behaviour landBehaviour;

	public override void begin ()
	{
		base.begin ();

		const bool b = false;
		_machine.animator.applyRootMotion = b;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		if(landBehaviour == null)
		{
			landBehaviour = _machine.animator.GetBehaviour<Land_Behaviour> ();
			landBehaviour.onStateExitCallback+= OnStateExitLand;
		}

		float landForce = _machine.animator.GetFloat("airSpeed");
		float horizontaAir = _machine.animator.GetFloat("airSpeedHorizontal");

		if (landForce < context.CharacterSettings.interruptForceOnLand) 
		{
			if (Mathf.Abs( horizontaAir ) > context.CharacterSettings.landHorizontalForceToRoll)
			{
				_machine.changeState<RollState> ();
				return;
			}
			else
				CrossFade("Land_Tree", 0.03f, 0.0f);
		}
		else 
		{
			if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) > 0.1f)
				_machine.changeState<WalkState> ();
			else
				_machine.changeState<IdleState> ();
		}

		if(context.CharacterSettings.stopMovementOnLand)
			context.CharacterMotor.SetVelocity (Vector3.zero);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public void OnStateExitLand()
	{
		if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) > 0.1f)
			_machine.changeState<WalkState> ();
		else
			_machine.changeState<IdleState> ();
	}


}
