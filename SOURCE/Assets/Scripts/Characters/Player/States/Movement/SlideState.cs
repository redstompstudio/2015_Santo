using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class SlideState : SKMecanimState<PlayerCharacterController>  
{
	private Slide_Behaviour slideBehaviour;

	private float slideSpeed;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		if (slideBehaviour == null)
			slideBehaviour = _machine.animator.GetBehaviour<Slide_Behaviour> ();

		slideBehaviour.onStateExitCallback += OnStateExitSlide;
		slideSpeed = context.CharacterSettings.slideSpeed * Mathf.Sign (context.Forward.x);

		CrossFade ("Slide", 0.04f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		Vector3 velocity = context.CharacterMotor.Velocity;

		velocity.x = slideSpeed;
		slideSpeed += (deltaTime * context.CharacterSettings.slideSpeedDecreaseRate * -Mathf.Sign (context.Forward.x));

		context.CharacterMotor.SetVelocity (velocity);
	}

	void OnStateExitSlide ()
	{
		_machine.changeState<IdleState> ();
	}
}
