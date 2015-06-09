using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CrouchState : SKMecanimState<PlayerCharacterController>  
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		Vector3 size = context.CharacterMotor.InitialColliderSize;
		size.y = context.CharacterSettings.crouchColliderSizeY;
		context.CharacterMotor.ResizeCollider (size);

		CrossFade ("Crouch", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if (!context.CharacterMotor.IsTouchingCeiling ()) 
		{
			if(!Input.GetKey(KeyCode.S))
			{
				_machine.changeState<IdleState> ();
				return;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			_machine.changeState<SlideState> ();
			return;
		}
		else if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			_machine.changeState<KnifeAttackState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		float horizontalInput = Input.GetAxisRaw ( "Horizontal" );

		if(Mathf.Abs(horizontalInput) > 0.0f)
		{
			context.CharacterMotor.RotateToDirection ( Vector3.right * horizontalInput );
		}
	}
}
