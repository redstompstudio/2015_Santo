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

        if (dashBackBehaviour == null)
            dashBackBehaviour = _machine.animator.GetBehaviour<DashBack_Behaviour> ();

        dashBackBehaviour.onStateExitCallback += OnStateExitRoll;

		rollVelocity = context.CharacterMotor.Velocity;
		rollVelocity.x = context.CharacterMotor.motorSettings.maxRunSpeed * -Mathf.Sign (context.Forward.x);

        CrossFade ("DashBack", 0.04f, 0.0f);
    }

    public override void update (float deltaTime, AnimatorStateInfo stateInfo)
    {
		rollVelocity = context.CharacterMotor.Velocity;
		rollVelocity.x = context.CharacterMotor.motorSettings.maxRunSpeed * -Mathf.Sign (context.Forward.x);

        context.CharacterMotor.SetVelocity (rollVelocity);
    }

    #region STATE_BEHAVIOUR ROLL
    public void OnStateExitRoll()
    {
        _machine.changeState<IdleState> ();
    }
    #endregion
}
