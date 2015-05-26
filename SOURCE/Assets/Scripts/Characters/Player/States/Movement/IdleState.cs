using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class IdleState : SKMecanimState<PlayerCharacterController> 
{
	public override void begin ()
	{
		base.begin ();

		CrossFade (context.GetStateTransition ("Idle"));

		//context.CharacterMotor.Move (Vector3.zero, 0.0f);
		Vector3 velocity = context.CharacterMotor.Velocity;
		velocity.x = 0.0f;

		context.CharacterMotor.SetVelocity (velocity);

		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
	}

	public override void reason ()
	{
		base.reason ();

		if(!context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<OnAirState> ();	
			return;
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			OnJumpInput ();
		}
		else if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			OnAttackInput ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.Mouse1))
		{
			OnAimInput ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.W))
		{
			if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, context.ClimbSettings.objectsMasks)) 
			{
				_machine.changeState<GrabLedgeState> ();
				return; 
			}
		}

		if(Mathf.Abs(Input.GetAxisRaw ("Horizontal")) > 0.0f)
		{
			_machine.changeState<WalkState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public void OnJumpInput()
	{
		if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, context.ClimbSettings.objectsMasks)) 
		{
//				_machine.changeState<GrabLedgeState> ();
		}
		else
		{
			_machine.changeState<JumpState> ();
		}
	}

	public void OnAttackInput()
	{
		_machine.changeState<FistsAttackState> ();
	}

	public void OnAimInput()
	{
		_machine.changeState<BowArrowState>();
	}
}
