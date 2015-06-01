using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class OnAirState :  SKMecanimState<PlayerCharacterController>
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		CrossFade ("OnAir_Tree", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if(context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<LandState>();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(Raycaster.HitSomething(context.CharCenterPoint, context.Forward, 1.0f, context.CharacterSettings.wallJumpLayers))
			{
				_machine.changeState<WallJumpState> ();
				return;
			}
		}
		else if(Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Mouse0))
		{
			_machine.changeState<FistSlamState> ();
			return;
		}
		else if(Input.GetKey(KeyCode.W))
		{
			Raycaster.RaycastHitInfo hitInfo = Raycaster.GetRaycastHitInfo (context.CharCenterPoint, context.CachedTransform.forward, 0.5f, 
				context.CharacterSettings.climbEdgeLayers);

			if (hitInfo.hitSomething) 
			{
				Vector3 climbPoint = hitInfo.hit.point;

				if (hitInfo.hitSomething)
					climbPoint = ClimbHelpers.GetColliderClimbPoint (context.Position, hitInfo.hit.collider);

				float distFromFeet = Mathf.Abs (climbPoint.y - context.Position.y);

				Debug.Log (distFromFeet);

				if (distFromFeet < 1.5f) {
					_machine.changeState<GrabLedgeState> ();
					return;	
				}
			}
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		if(context.CharacterSettings.hasAirControl)
		{
			context.CharacterMotor.Move (new Vector3(Input.GetAxisRaw ("Horizontal"), 0.0f, 0.0f), context.CharacterSettings.maxRunSpeed);
			context.CharacterMotor.RotateToVelocityDirection (20.0f);
		}

		float airSpeed = ValuesMapping.Map (context.CharacterMotor.Velocity.y, -15.0f, 15.0f, -1.0f, 1.0f);

		float maxHorizontalSpeed = context.CharacterSettings.maxRunSpeed;
		float airSpeedHorizontal = ValuesMapping.Map (context.CharacterMotor.Velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed, -1.0f, 1.0f);

		_machine.animator.SetFloat ("airSpeed", airSpeed);
		_machine.animator.SetFloat ("airSpeedHorizontal", airSpeedHorizontal);
	}
}
