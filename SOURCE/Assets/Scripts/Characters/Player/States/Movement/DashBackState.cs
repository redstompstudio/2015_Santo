using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class DashBackState : SKMecanimState<PlayerCharacterController> 
{
    private DashBack_Behaviour dashBackBehaviour;
    private Vector3 rollVelocity; 

    public override void begin ()
    {
        base.begin ();

        _machine.animator.applyRootMotion = false;

		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.IsKinematic = false;

		Vector3 size = context.CharacterMotor.InitialColliderSize;
		size.y = context.CharacterSettings.dashBackColliderSizeY;
		context.CharacterMotor.ResizeCollider (size);

        if (dashBackBehaviour == null)
            dashBackBehaviour = _machine.animator.GetBehaviour<DashBack_Behaviour> ();

        dashBackBehaviour.onStateExitCallback = OnStateExitRoll;

        CrossFade ("DashBack", 0.04f, 0.0f);
    }

    public override void update (float deltaTime, AnimatorStateInfo stateInfo)
    {
		rollVelocity = context.CharacterMotor.Velocity;
		rollVelocity.x = context.CharacterSettings.dashBackSpeed * -Mathf.Sign (context.Forward.x);

        context.CharacterMotor.SetVelocity (rollVelocity);
    }

    #region STATE_BEHAVIOUR ROLL
    public void OnStateExitRoll()
    {
		if (context.CharacterMotor.IsTouchingCeiling()) 
			_machine.changeState<CrouchState> ();
		else
			_machine.changeState<IdleState> ();
    }
    #endregion
}
