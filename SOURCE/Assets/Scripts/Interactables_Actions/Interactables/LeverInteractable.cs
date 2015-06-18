using UnityEngine;
using System.Collections;

public class LeverInteractable : BaseInteractable 
{
	public BaseAction door;

	public override void OnStartedInteraction ()
	{
		base.OnStartedInteraction ();

		if (!canInteract)
			return;

		door.StartAction ();
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if(door != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine (transform.position, door.transform.position);
		}
	}
	#endif
}
