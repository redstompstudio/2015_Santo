using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class WalkState :  SKMecanimState<PlayerCharacterController>
{
	private float horizontalInput;

	public override void begin ()
	{
		base.begin ();

		CrossFade (context.GetStateTransition ("Run_Tree"));

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
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
			if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, context.ClimbSettings.objectsMasks)) 
			{
//					_machine.changeState<GrabLedgeState> ();
				return;
			}
			else
			{
				_machine.changeState<JumpState> ();
				return;
			}
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			_machine.changeState<RollState> ();
			return;
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			_machine.changeState<SlideState> ();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
//			_machine.changeState<AttackState> ();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Mouse1))
		{
			_machine.changeState<BowArrowState>();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		context.CharacterMotor.Move (new Vector3(horizontalInput, 0.0f, 0.0f), context.CharacterMotor.MaxRunSpeed);
		context.CharacterMotor.RotateToVelocityDirection (50.0f);
	}
}
