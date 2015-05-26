using UnityEngine;
using System.Collections;

public class BaseStateMachineBehaviour : StateMachineBehaviour
{
	public delegate void OnStateEnterDelegate();
	public OnStateEnterDelegate onStateEnterCallback = null;

	public delegate void OnStateUpdateDelegate();
	public OnStateUpdateDelegate onStateUpdateCallback = null;

	public delegate void OnStateExitDelegate();
	public OnStateExitDelegate onStateExitCallback = null;

	public delegate void OnStateIKDelegate ();
	public OnStateIKDelegate onStateIKCallback;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if(onStateEnterCallback != null)
			onStateEnterCallback ();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (onStateUpdateCallback != null)
			onStateUpdateCallback ();
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (onStateExitCallback != null)
			onStateExitCallback ();
	}
}
