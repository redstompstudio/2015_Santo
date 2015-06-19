using UnityEngine;
using Prime31.StateKit;

public class MonkeyMeleeAttackState : SKMecanimState<PossessedMonkeyController>  
{
	public Fist_PunchL_Behaviour punchLBehaviour;

	public override void begin ()
	{
		base.begin ();

		if(punchLBehaviour == null)
		{
			punchLBehaviour = _machine.animator.GetBehaviour<Fist_PunchL_Behaviour> ();
			punchLBehaviour.onStateExitCallback += OnStateExitPunchL;
		}

		CrossFade ("Attack", 0.1f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	private void OnStateExitPunchL ()
	{
		_machine.changeState<MonkeyIdleState> ();
		return;
	}

#if UNITY_EDITOR
	public override void OnGizmos ()
	{
		base.OnGizmos ();
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (context.Position + Vector3.up * 2, Vector3.one * 0.7f);
	}
#endif
}
