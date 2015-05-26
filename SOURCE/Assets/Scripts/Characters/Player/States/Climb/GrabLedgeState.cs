using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class GrabLedgeState : SKMecanimState<PlayerCharacterController>
{
	private ClimbMechanicsSettings climbSettings;

	private EdgeGrab_Behaviour edgeGrabBehaviour;
	private EdgeGrabClimb_Behaviour edgeGrabClimbBehaviour;

	private Vector3 climbPoint;
	private bool isClimbing;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = true;
		context.CharacterMotor.IsKinematic = true;
		context.CharacterMotor.UseGravity = false;

		if (climbSettings == null)
			climbSettings = context.ClimbSettings;

		if (edgeGrabBehaviour == null) 
		{
			edgeGrabBehaviour = _machine.animator.GetBehaviour<EdgeGrab_Behaviour> ();

			edgeGrabBehaviour.onStateEnterCallback += OnStateEnterGrab;
			edgeGrabBehaviour.onStateUpdateCallback += OnStateUpdateGrab;
			edgeGrabBehaviour.onStateExitCallback += OnStateExitGrab;
		}

		if (edgeGrabClimbBehaviour == null) 
		{
			edgeGrabClimbBehaviour = _machine.animator.GetBehaviour<EdgeGrabClimb_Behaviour> ();

			edgeGrabClimbBehaviour.onStateEnterCallback += OnStateEnterGrabClimb;
			edgeGrabClimbBehaviour.onStateExitCallback += OnStateExitGrabClimb;
		}
	
		Raycaster.RaycastHitInfo hitInfo = Raycaster.GetRaycastHitInfo (context.CharCenterPoint, context.CachedTransform.forward, 1.5f, 
			climbSettings.objectsMasks);

		if (hitInfo.hitSomething)
			climbPoint = ClimbHelpers.GetColliderClimbPoint (context.Position, hitInfo.hit.collider);

		float distFromFeet = Mathf.Abs(climbPoint.y - context.Position.y);
		_machine.animator.SetFloat ("climbHeight", distFromFeet);

		CrossFade ("EdgeGrab_Start_Tree", 0.03f, 0.0f);
//		CrossFade ("EdgeGrab_Start", 0.03f, 0.0f);

		isClimbing = false;
	}

	public override void reason ()
	{
		base.reason ();

		if(climbSettings.waitForInputToClimb) 
		{
			if (!isClimbing && Input.GetKeyDown (KeyCode.W)) 
				CrossFade ("EdgeGrab_Climb_Tree", 0.03f, 0.0f);
//				CrossFade ("EdgeGrab_Climb", 0.03f, 0.0f);
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	#region STATE_BEHAVIOURS GRAB
	public void OnStateEnterGrab()
	{
	}

	public void OnStateUpdateGrab()
	{
		if(!_machine.animator.IsInTransition(0))
		{
			_machine.animator.MatchTarget (climbPoint, context.Rotation,
				AvatarTarget.LeftHand, new MatchTargetWeightMask (new Vector3(0.0f, 1.0f, 1.0f), 0.0f), 
				climbSettings.matchTargetStartNormalizedTime, climbSettings.matchTargetNormalizedTime);

			Debug.DrawLine (context.Position, climbPoint, Color.blue, Time.deltaTime);
		}
	}

	public void OnStateExitGrab()
	{
		if (!climbSettings.waitForInputToClimb) {
			CrossFade ("EdgeGrab_Climb_Tree", 0.03f, 0.0f);
//			CrossFade ("EdgeGrab_Climb", 0.03f, 0.0f);
		}
	}
	#endregion 

	#region STATE_BEHAVIOURS STATE_CLIMB
	public void OnStateEnterGrabClimb()
	{
		isClimbing = true;
	}

	public void OnStateExitGrabClimb()
	{
		_machine.animator.applyRootMotion = false;

		if (Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > 0.0f)
			_machine.changeState<WalkState> ();
		else
			_machine.changeState<IdleState> ();
	}
	#endregion
}
