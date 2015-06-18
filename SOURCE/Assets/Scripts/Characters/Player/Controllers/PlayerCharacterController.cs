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
			
		};
#endif	
	}

	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.J))
			Debug.Break ();
#endif

		if(Input.GetKeyDown(KeyCode.H))
			Reset ();

		stateMachine.update( Time.deltaTime );

		if (Health.currentHealth <= 0.0f)
			stateMachine.changeState<DeadState> ();

		if(Input.GetKeyDown(KeyCode.F5))
		{
			XMLSerializer.Save<CharacterSettings>(CharacterSettings, "Player_CharSettings.xml");
		}
		else if(Input.GetKeyDown(KeyCode.F9))
		{
			CharacterSettings = XMLSerializer.Load<CharacterSettings> ("Player_CharSettings.xml");
		}
	}

	void FixedUpdate()
	{
		stateMachine.fixedUpdate (Time.deltaTime);
	}

	protected override void Reset ()
	{
		base.Reset ();
		stateMachine.changeState<IdleState> ();

		CachedTransform.position = CheckpointManager.Instance.CurrentCheckpoint.transform.position;
	}

	public override void Kill ()
	{
		base.Kill ();
		CachedGameObject.SetActive (false);
	}

#if UNITY_EDITOR
	void OnGUI()
	{
		if(stateMachine != null)
			GUILayout.Box (stateMachine.currentState.ToString());
	}

	void OnDrawGizmos()
	{
		if(stateMachine != null)
			stateMachine.OnGizmos ();
	}
#endif
}
