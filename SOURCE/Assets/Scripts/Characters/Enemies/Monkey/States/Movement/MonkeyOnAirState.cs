using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class MonkeyOnAirState : SKMecanimState<BaseEnemy> 
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
			_machine.changeState<MonkeyLandState>();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		float airSpeed = ValuesMapping.Map (context.CharacterMotor.Velocity.y, -15.0f, 15.0f, -1.0f, 1.0f);

		float maxHorizontalSpeed = context.CharacterSettings.maxRunSpeed;
		float airSpeedHorizontal = ValuesMapping.Map (context.CharacterMotor.Velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed, -1.0f, 1.0f);

		_machine.animator.SetFloat ("airSpeed", airSpeed);
		_machine.animator.SetFloat ("airSpeedHorizontal", airSpeedHorizontal);
	}
}
