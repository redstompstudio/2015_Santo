using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class WalkState :  SKMecanimState<PlayerCharacterController>
{
	private float horizontalInput;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		context.CharacterMotor.ResetColliderValues ();

		CrossFade("Run_Tree", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		horizontalInput = Input.GetAxisRaw ("Horizontal");

		if(!context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<OnAirState> ();	
			return;
		}

		if(Mathf.Abs(horizontalInput) == 0.0f)
		{
			_machine.changeState<IdleState> ();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			_machine.changeState<JumpState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.F))
		{
			_machine.changeState<RollState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.W))
		{
			if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, context.CharacterSettings.climbEdgeLayers)) 
			{
				_machine.changeState<GrabLedgeState> ();
				return; 
			}
		}
		else if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			_machine.changeState<FistsAttackState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.Mouse1))
		{
			if(Input.GetKey(KeyCode.LeftShift))
				_machine.changeState<BowArrowState>();
			else
				_machine.changeState<RifleAimState>();
			
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		context.CharacterMotor.Move (new Vector3(horizontalInput, 0.0f, 0.0f), context.CharacterSettings.maxRunSpeed * deltaTime);
		context.CharacterMotor.RotateToVelocityDirection (float.PositiveInfinity);
	}
}
