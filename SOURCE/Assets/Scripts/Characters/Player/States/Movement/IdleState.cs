using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class IdleState : SKMecanimState<PlayerCharacterController> 
{
	private Idle_Behaviour idleBehaviour;
	private LayerMask climbEdgeLayers;

	private Raycaster.RaycastHitInfo handRayInfo;
	private float horizontalInput;

	private bool useHandIK;
	private Vector3 handPosition;

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

		//idleBehaviour.onStateIKCallback += OnStateIKIdle;

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
			handRayInfo = Raycaster.GetRaycastHitInfo (context.CharCenterPoint, 
				context.Forward * horizontalInput, 1.0f, context.CharacterSettings.wallJumpLayers);

			if(handRayInfo.hitSomething)
			{
				useHandIK = true;
				handPosition = handRayInfo.hit.point + Vector3.up * 0.7f;
			} 
			else 
			{
				useHandIK = false;
				_machine.changeState<WalkState> ();
				return;
			}
		}
		else
		{
			useHandIK = false;
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

	private void OnStateIKIdle()
	{
		if(useHandIK)
		{
			Debug.Log ("IK");
			Vector3 curHandPos = _machine.animator.GetIKPosition (AvatarIKGoal.LeftHand);
			curHandPos = Vector3.Lerp (curHandPos, handPosition, Time.deltaTime * 10.0f);

			_machine.animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1.0f);
			_machine.animator.SetIKPosition(AvatarIKGoal.LeftHand, curHandPos);
		}
	}
}
