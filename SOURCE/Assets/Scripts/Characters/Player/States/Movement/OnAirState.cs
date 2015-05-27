﻿using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class OnAirState :  SKMecanimState<PlayerCharacterController>
{
	public override void begin ()
	{
		base.begin ();

		CrossFade (context.GetStateTransition ("OnAir_Tree"));
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
			if(Raycaster.HitSomething(context.CharCenterPoint, context.Forward, 1.0f, context.wallJumpLayersMask))
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

		/*
		if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
		{
			if (Raycaster.HitSomething (context.CharCenterPoint, context.Forward, 1.5f, context.ClimbSettings.objectsMasks)) 
			{
				Raycaster.RaycastHitInfo hitInfo = Raycaster.GetRaycastHitInfo (context.CharCenterPoint, context.CachedTransform.forward, 1.5f, 
					context.ClimbSettings.objectsMasks);

				Vector3 climbPoint = hitInfo.hit.point;

				if (hitInfo.hitSomething)
					climbPoint = ClimbHelpers.GetColliderClimbPoint (context.Position, hitInfo.hit.collider);

				float distFromFeet = Mathf.Abs(climbPoint.y - context.Position.y);

				Debug.Log (distFromFeet);

				if(distFromFeet < 1.5f)
				{
					_machine.changeState<GrabLedgeState> ();
					return;	
				}
			}
		}
		*/
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		if(context.GamePlaySettings.hasAirControl)
		{
			context.CharacterMotor.Move (new Vector3(Input.GetAxisRaw ("Horizontal"), 0.0f, 0.0f), context.CharacterMotor.MaxRunSpeed);
			context.CharacterMotor.RotateToVelocityDirection (20.0f);
		}

		float airSpeed = ValuesMapping.Map (context.CharacterMotor.Velocity.y, -20.0f, 20.0f, -1.0f, 1.0f);
		float airSpeedHorizontal = ValuesMapping.Map (context.CharacterMotor.Velocity.x, -10.0f, 10.0f, -1.0f, 1.0f);

		_machine.animator.SetFloat ("airSpeed", airSpeed);
		_machine.animator.SetFloat ("airSpeedHorizontal", airSpeedHorizontal);
	}
}
