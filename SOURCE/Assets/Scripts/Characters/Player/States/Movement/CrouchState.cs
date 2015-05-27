using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class CrouchState : SKMecanimState<PlayerCharacterController>  
{
	public override void begin ()
	{
		base.begin ();

		CrossFade ("Crouch", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if(!Input.GetKey(KeyCode.S))
		{
			_machine.changeState<IdleState> ();
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			_machine.changeState<SlideState> ();
			return;
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}


}
