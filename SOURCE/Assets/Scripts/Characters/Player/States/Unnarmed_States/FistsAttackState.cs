using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class FistsAttackState : SKMecanimState<PlayerCharacterController>  
{
	private Fist_HighPunch_L_Behaviour fistHighPunch_L_Behaviour;
	private Fist_HighPunch_R_Behaviour fistHighPunch_R_Behaviour;
	private ChargedPunch_Behaviour chargedPunchBehaviour;

	private bool executeNextAttack;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.StopMovement (true, false);

		if(fistHighPunch_L_Behaviour == null)
			fistHighPunch_L_Behaviour = _machine.animator.GetBehaviour<Fist_HighPunch_L_Behaviour> ();

		if(fistHighPunch_R_Behaviour == null)
			fistHighPunch_R_Behaviour = _machine.animator.GetBehaviour<Fist_HighPunch_R_Behaviour> ();

		if (chargedPunchBehaviour == null)
			chargedPunchBehaviour = _machine.animator.GetBehaviour<ChargedPunch_Behaviour> ();

		fistHighPunch_L_Behaviour.onStateEnterCallback = OnStateEnterHighPunch_L;
		fistHighPunch_L_Behaviour.onStateUpdateCallback = OnStateUpdateHighPuch_L;
		fistHighPunch_L_Behaviour.onStateExitCallback = OnStateExitHighPunch_L;

		fistHighPunch_R_Behaviour.onStateEnterCallback = OnStateEnterHighPunch_R;
		fistHighPunch_R_Behaviour.onStateUpdateCallback = OnStateUpdateHighPuch_R;
		fistHighPunch_R_Behaviour.onStateExitCallback = OnStateExitHighPunch_R;

		chargedPunchBehaviour.onStateEnterCallback = OnStateEnterChagedPunch;
		chargedPunchBehaviour.onStateExitCallback = OnStateExitChagedPunch;

		CrossFade ("Fist_Punch_L", 0.01f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	#region STATE_BEHAVIOUR FIST_L
	public void OnStateEnterHighPunch_L()
	{
		executeNextAttack = false;
	}

	public void OnStateUpdateHighPuch_L ()
	{
		executeNextAttack |= Input.GetKeyDown (KeyCode.Mouse0);
	}

	public void OnStateExitHighPunch_L()
	{
		if (executeNextAttack) 
			CrossFade ("Fist_Punch_R", 0.03f, 0.0f);
		else
			_machine.changeState<IdleState> ();
	}
	#endregion

	#region STATE_BEHAVIOUR FIST_R
	public void OnStateEnterHighPunch_R()
	{
		executeNextAttack = false;
	}

	public void OnStateUpdateHighPuch_R()
	{
		executeNextAttack |= Input.GetKeyDown (KeyCode.Mouse0);
	}

	public void OnStateExitHighPunch_R()
	{
		if(executeNextAttack)
			CrossFade ("Charged_Punch", 0.03f, 0.0f);
		else
			_machine.changeState<IdleState> ();
	}
	#endregion

	#region STATE_BEHAVIOUR CHARGED_PUNCh
	public void OnStateEnterChagedPunch()
	{
		executeNextAttack = false;
	}

	public void OnStateExitChagedPunch()
	{
		_machine.changeState<IdleState> ();
	}
	#endregion
}