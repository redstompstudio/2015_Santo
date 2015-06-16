using UnityEngine;
using System.Collections;
using Prime31.StateKit;

[RequireComponent(typeof(AIController))]
public class BaseEnemyController : BaseCharacterController 
{
	protected SKMecanimStateMachine<BaseEnemyController> charStateMachine;

	public AIController aiController;
	[HideInInspector]
	public AttackController attackController;

	[Header("RANGES SETTINGS")]
	public TriggerHandler visionRange;

	protected override void Awake ()
	{
		base.Awake ();

		if (aiController == null)
			aiController = new AIController();

		if(visionRange != null)
		{
			visionRange.onTriggerEnterCallback += OnEnterVisionRange;
			visionRange.onTriggerExitCallback += OnExitVisionRange;
		}
	}

	protected override void Start ()
	{
		base.Start ();

		charStateMachine = new SKMecanimStateMachine<BaseEnemyController> (GetComponent<Animator> (), this, new MonkeyIdleState());

		charStateMachine.addState (new MonkeyOnAirState ());
		charStateMachine.addState (new MonkeyLandState ());
		charStateMachine.addState (new MonkeyScoutState ());

		#if UNITY_EDITOR
		charStateMachine.onStateChanged += () =>
		{
			//Debug.Log(charStateMachine.currentState.ToString());
		};
		#endif	
	}

	void Update()
	{
		charStateMachine.update( Time.deltaTime );
	}

	void FixedUpdate()
	{
		charStateMachine.fixedUpdate (Time.deltaTime);
	}

	#region VISION TRIGGER
	private void OnEnterVisionRange()
	{
		Debug.Log ("Enter Vision Range");
	}

	private void OnExitVisionRange()
	{
		Debug.Log ("Exit Vision Range");
	}
	#endregion

	#if UNITY_EDITOR
	void OnGUI()
	{
		if(charStateMachine != null)
			GUI.Box (new Rect(Screen.width * 0.75f, 0.0f, Screen.width * 0.25f, Screen.height * 0.2f), charStateMachine.currentState.ToString());
	}

	public void OnDrawGizmos()
	{
		if(charStateMachine != null)
			charStateMachine.OnGizmos();
	}
	#endif
}
