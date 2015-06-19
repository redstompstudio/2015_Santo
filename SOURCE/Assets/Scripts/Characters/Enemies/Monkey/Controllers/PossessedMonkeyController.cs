using UnityEngine;
using Prime31.StateKit;

public class PossessedMonkeyController : BaseEnemy
{
	private SKMecanimStateMachine<PossessedMonkeyController> stateMachine;

	protected override void Start ()
	{
		base.Start ();

		stateMachine = new SKMecanimStateMachine<PossessedMonkeyController>( GetComponent<Animator>(), this, new MonkeyScoutState() );

		stateMachine.addState (new MonkeyScoutState ());
		stateMachine.addState (new MonkeyIdleState());
		stateMachine.addState (new MonkeyChaseState ());
		stateMachine.addState (new MonkeyMeleeAttackState ());
	}

	void Update()
	{
		stateMachine.update (Time.deltaTime);
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (stateMachine != null)
			stateMachine.OnGizmos ();
	}
#endif
}
