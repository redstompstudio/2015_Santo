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
			for(int i = -1; i < 2; i++)
			{
				Vector3 offset = ((context.CharacterMotor.CachedCollider.size / 2.0f) * i);
				offset.z = 0.0f;
				offset.x = 0.0f;

				Vector3 point = context.CharCenterPoint + offset;
#if UNITY_EDITOR
				Debug.DrawRay(context.CharCenterPoint, Vector3.up, Color.magenta, 4.0f);
				Debug.DrawRay (point, context.Forward * context.CharacterSettings.maxDistanceFromWall, Color.blue, 4.0f);
#endif
				if(Raycaster.HitSomething(point, context.Forward, context.CharacterSettings.maxDistanceFromWall, context.CharacterSettings.wallJumpLayers))
				{
					_machine.changeState<WallJumpState> ();
					return;
				}
			}
		}
		else if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			if(Input.GetKey(KeyCode.S))
			{
				//_machine.changeState<FistSlamState> ();
				return;
			}
			else
			{
				_machine.changeState<KnifeAttackState> ();
				return;
			}
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

#if UNITY_EDITOR
				Debug.Log (distFromFeet);
#endif

				if (distFromFeet < 1.5f) {
					_machine.changeState<GrabLedgeState> ();
					return;	
				}
			}
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		if(context.CharacterSettings.enableAirControl)
		{
			Vector3 direction = Vector3.zero;
			direction.x = Input.GetAxisRaw ("Horizontal");

			context.CharacterMotor.Move (new Vector3(direction.x * context.CharacterSettings.maxRunSpeed * context.CharacterSettings.airSpeedRatio, 
				context.CharacterMotor.Velocity.y, 0.0f), 
				deltaTime);
			
			context.CharacterMotor.RotateToDirection(direction);
		}

		float airSpeed = ValuesMapping.Map (context.CharacterMotor.Velocity.y, -5.0f, 5.0f, -1.0f, 1.0f);

		float maxHorizontalSpeed = context.CharacterSettings.maxRunSpeed;
		float airSpeedHorizontal = ValuesMapping.Map (context.CharacterMotor.Velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed, -1.0f, 1.0f);

		_machine.animator.SetFloat ("airSpeed", airSpeed);
		_machine.animator.SetFloat ("airSpeedHorizontal", airSpeedHorizontal);
	}
}
