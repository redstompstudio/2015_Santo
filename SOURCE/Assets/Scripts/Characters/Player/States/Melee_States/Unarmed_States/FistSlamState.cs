using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class FistSlamState : SKMecanimState<PlayerCharacterController>   
{
	private Fist_AirSlam_Behaviour fistAirSlamBehaviour;
	private Vector3 slamVelocity;

	private bool isSmashing = false;

	public override void begin ()
	{
		base.begin ();

		if (fistAirSlamBehaviour == null)
			fistAirSlamBehaviour = _machine.animator.GetBehaviour<Fist_AirSlam_Behaviour> ();

		fistAirSlamBehaviour.onStateExitCallback = OnStateExitAirSlamSmash;

		isSmashing = false;
		slamVelocity = new Vector3 (0.0f, -10.0f, 0.0f);
		CrossFade ("Attack_Fist_Slam_Air_Load", 0.03f, 0.0f);
	}

	public override void reason ()
	{
		base.reason ();

		if (!isSmashing) 
		{
			if (context.CharacterMotor.IsGrounded) 
			{
				isSmashing = true;
				CrossFade ("Attack_Fist_Slam_Air_Smash", 0.03f, 0.0f);
				return;
			}
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	public override void fixedUpdate (float deltaTime, AnimatorStateInfo stateInfo)
	{
		base.fixedUpdate (deltaTime, stateInfo);
		context.CharacterMotor.SetVelocity (slamVelocity);
	}

	public void OnStateExitAirSlamSmash()
	{
		_machine.changeState<CrouchState> ();
	}
}
