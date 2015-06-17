using UnityEngine;
using System.Collections;

public class DoorActions : BaseAction
{
	public Transform openingPosition;
	public float openSpeed = 5.0f;


	public override void StartAction ()
	{
		base.StartAction ();
		StartCoroutine (OpenDoor ());
	}

	public override void StopAction ()
	{
		base.StopAction ();
		StopCoroutine (OpenDoor());
	}

	private IEnumerator OpenDoor()
	{
		yield return new WaitForSeconds (actionDelay);

		float dist = Vector3.Distance (openingPosition.position, CachedTransform.position);

		while(dist > 0.1f)
		{
			CachedTransform.position = Vector3.MoveTowards(CachedTransform.position, openingPosition.position, Time.deltaTime * openSpeed);
			dist = Vector3.Distance (openingPosition.position, CachedTransform.position);
			yield return null;
		}
	}
}
