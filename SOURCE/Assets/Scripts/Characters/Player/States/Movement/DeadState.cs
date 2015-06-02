using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class DeadState : SKMecanimState<PlayerCharacterController>  
{
	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = true;
		context.CharacterMotor.UseGravity = true;

		CrossFade ("Death", 0.03f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}
}
