using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.StateKit;

[System.Serializable]
public class StateTransitions
{
	public string stateName;

	[Range(0.0f, 5.0f)]
	public float transitionTime = 0.03f;

	[Range(0.0f, 1.0f)]
	public float normalizedTime = 0.0f;
}

public class PlayerCharacterController : BaseCharacterController 
{
	public List<StateTransitions> stateTransitions = new List<StateTransitions>();
	private Dictionary<string, StateTransitions> stateTransitionsDictionary = new Dictionary<string, StateTransitions>();

	private SKMecanimStateMachine<PlayerCharacterController> stateMachine;

	[SerializeField]
	private ClimbMechanicsSettings climbMechanicsSettings;

	public LayerMask wallJumpLayersMask;

	#region PROPERTIES

	public ClimbMechanicsSettings ClimbSettings{
		get{
			if (climbMechanicsSettings == null)
				climbMechanicsSettings = new ClimbMechanicsSettings();
			return climbMechanicsSettings;
		}
	}
	#endregion

	protected override void Awake ()
	{
		base.Awake ();

		for (int i = 0; i < stateTransitions.Count; i++)
			stateTransitionsDictionary.Add (stateTransitions [i].stateName, stateTransitions [i]);

		if (climbMechanicsSettings == null)
			climbMechanicsSettings = new ClimbMechanicsSettings ();
	}

	protected override void Start ()
	{
		base.Start ();

		stateMachine = new SKMecanimStateMachine<PlayerCharacterController>( GetComponent<Animator>(), this, new IdleState() );

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

		stateMachine.addState (new BowArrowState ());
		stateMachine.addState (new BowArrowFireState ());

		stateMachine.addState (new FistsAttackState ());

#if UNITY_EDITOR
		stateMachine.onStateChanged += () =>
		{
			Debug.Log(stateMachine.currentState.ToString());
		};
#endif	
	}

	void Update()
	{
		stateMachine.update( Time.deltaTime );
	}

	void FixedUpdate()
	{
		stateMachine.fixedUpdate (Time.deltaTime);
	}

	public StateTransitions GetStateTransition(string pStateName)
	{
		return stateTransitionsDictionary [pStateName];
	}

#if UNITY_EDITOR
	void OnGUI()
	{
		if(stateMachine != null)
			GUILayout.Box (stateMachine.currentState.ToString());
	}
#endif
}
