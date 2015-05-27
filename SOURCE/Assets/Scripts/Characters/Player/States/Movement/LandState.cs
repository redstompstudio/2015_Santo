using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class LandState : SKMecanimState<PlayerCharacterController>
{
	public Land_Behaviour landBehaviour;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		if(landBehaviour == null)
		{
			landBehaviour = _machine.animator.GetBehaviour<Land_Behaviour> ();
			landBehaviour.onStateExitCallback+= OnStateExitLand;
		}

		//_machine.animator.SetTrigger ("OnBeginLand");
		float landForce = _machine.animator.GetFloat("airSpeed");
		float horizontaAir = _machine.animator.GetFloat("airSpeedHorizontal");

		if (landForce < -0.25f) 
		{
			if (Mathf.Abs( horizontaAir ) > 0.4f)
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

		context.CharacterMotor.SetVelocity (Vector3.zero);
	}

	#region implemented abstract members of SKMecanimState
	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}
	#endregion

	public void OnStateExitLand()
	{
		if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) > 0.1f)
			_machine.changeState<WalkState> ();
		else
			_machine.changeState<IdleState> ();
	}


}
