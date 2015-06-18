using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	private void OnTriggerEnter(Collider pOther)
	{
		CheckpointManager.Instance.OnEnabledCheckpoint (this);
	}
}
