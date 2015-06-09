using UnityEngine;
using Prime31.StateKit;

public class PlayerCharacterController : BaseCharacterController 
{
	private SKMecanimStateMachine<PlayerCharacterController> stateMachine;
	[HideInInspector]
	public AttackController attackController;

	#region PROPERTIES
	#endregion

	protected override void Awake ()
	{
		base.Awake ();

		if (attackController == null)
			attackController = GetComponent<AttackController> ();
	}

	protected override void Start ()
	{
		base.Start ();

		stateMachine = new SKMecanimStateMachine<PlayerCharacterController>( GetComponent<Animator>(), this, new IdleState() );

		stateMachine.addState (new DeadState ());
		stateMachine.addState (new WalkState());
		stateMachine.addState (new JumpState ());
		stateMachine.addState (new OnAirState ());
		stateMachine.addState (new LandState ());

		stateMachine.addState (new CrouchState ());
		stateMachine.addState (new WallJumpState ());
		stateMachine.addState (new RollState ());
		stateMachine.addState (new DashBackState ());
		stateMachine.addState (new SlideState ());
		stateMachine.addState (new GrabLedgeState ());

		stateMachine.addState (new RifleAimState ());
		stateMachine.addState (new RifleFireState ());

		stateMachine.addState (new KnifeAttackState());

		stateMachine.addState (new PullGroundLeverState ());

#if UNITY_EDITOR
		stateMachine.onStateChanged += () =>
		{
			//Debug.Log(stateMachine.currentState.ToString());
		};
#endif	
	}

	void Update()
	{
		stateMachine.update( Time.deltaTime );

		if (Health.currentHealth <= 0.0f)
			stateMachine.changeState<DeadState> ();
	}

	void FixedUpdate()
	{
		stateMachine.fixedUpdate (Time.deltaTime);
	}

#if UNITY_EDITOR
	void OnGUI()
	{
		if(stateMachine != null)
			GUILayout.Box (stateMachine.currentState.ToString());
	}
#endif
}
