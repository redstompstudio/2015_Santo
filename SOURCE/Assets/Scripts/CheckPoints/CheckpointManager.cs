using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour 
{
	private static CheckpointManager instance;

	private Checkpoint currentCheckpoint;

	public static CheckpointManager Instance{
		get{
			if (instance == null)
				instance = FindObjectOfType (typeof(CheckpointManager)) as CheckpointManager;
			
			return instance;
		}
	}

	public Checkpoint CurrentCheckpoint{
		get{
			return currentCheckpoint;
		}
	}

	public void OnEnabledCheckpoint(Checkpoint pPoint)
	{
		currentCheckpoint = pPoint;	
	}
}
