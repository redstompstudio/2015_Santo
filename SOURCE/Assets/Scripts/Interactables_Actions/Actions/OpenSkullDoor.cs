using UnityEngine;
using System.Collections;

public class OpenSkullDoor : BaseAction
{
	public Action_PlayAnimation doorAnimation;

	public override void StartAction ()
	{
		base.StartAction ();

		StartCoroutine (OpenDoor ());
	}

	private IEnumerator OpenDoor()
	{
		yield return new WaitForSeconds (actionDelay);

		if(doorAnimation != null)
			doorAnimation.StartAction ();
	}
}

