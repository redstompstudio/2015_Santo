using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class BaseEnemyController : BaseCharacterController 
{
	protected SKMecanimStateMachine<BaseEnemyController> charStateMachine;

	[HideInInspector]
	public AttackController attackController;

	protected override void Start ()
	{
		base.Start ();

		charStateMachine = new SKMecanimStateMachine<BaseEnemyController> (GetComponent<Animator> (), this, new MonkeyIdleState());

		charStateMachine.addState (new MonkeyOnAirState ());
		charStateMachine.addState (new MonkeyLandState ());

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

	#if UNITY_EDITOR
	void OnGUI()
	{
		if(charStateMachine != null)
			GUI.Box (new Rect(Screen.width * 0.75f, 0.0f, Screen.width * 0.25f, Screen.height * 0.2f), charStateMachine.currentState.ToString());
	}
	#endif
}
