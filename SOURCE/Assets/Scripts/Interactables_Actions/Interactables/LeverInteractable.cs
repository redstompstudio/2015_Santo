using UnityEngine;
using System.Collections;

public class LeverInteractable : BaseInteractable 
{
	public BaseAction[] actions;

	public override void OnStartedInteraction ()
	{
		base.OnStartedInteraction ();

		if (!canInteract)
			return;

		foreach(BaseAction action in actions)
			action.StartAction ();
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if(actions != null)
		{
			Gizmos.color = Color.green;

			for(int i = 0; i < actions.Length; i++)
			{
				Gizmos.DrawLine (transform.position, actions[i].transform.position);	
			}
		}
	}
	#endif
}
