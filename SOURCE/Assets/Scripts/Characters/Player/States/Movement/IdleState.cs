using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class IdleState : SKMecanimState<PlayerCharacterController> 
{
	private Idle_Behaviour idleBehaviour;
	private LayerMask climbEdgeLayers;
	private float horizontalInput;

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

		if (idleBehaviour == null)
			idleBehaviour = _machine.animator.GetBehaviour<Idle_Behaviour> ();

		climbEdgeLayers = context.CharacterSettings.climbEdgeLayers;

		CrossFade ("Idle", 0.13f, 0.0f);
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
			_machine.changeState<JumpState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			_machine.changeState<KnifeAttackState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.Mouse1))
		{
			OnAimInput ();
			return;
		}
		else if(Input.GetKey(KeyCode.W))
		{
			if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, climbEdgeLayers)) 
			{
				_machine.changeState<GrabLedgeState> ();
				return; 
			}
		}
		else if(Input.GetKey(KeyCode.S))
		{
			_machine.changeState<CrouchState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.LeftControl))
		{
			_machine.changeState<DashBackState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.E))
		{
			_machine.changeState<PullGroundLeverState> ();
			return;
		}

		horizontalInput = Input.GetAxisRaw ("Horizontal");
		if(Mathf.Abs(horizontalInput) > 0.0f)
		{
			_machine.changeState<WalkState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public void OnAimInput()
	{
		if(Input.GetKey(KeyCode.LeftShift))
			_machine.changeState<BowArrowState>();
		else
			_machine.changeState<RifleAimState>();
	}
}
