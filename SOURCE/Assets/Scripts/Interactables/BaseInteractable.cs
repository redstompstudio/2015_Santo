using UnityEngine;
using System.Collections;

public class BaseInteractable : MonoBehaviour 
{
	public bool canInteract;

	public virtual void OnStartedInteraction ()
	{
	}

	public virtual void OnFinishedInteraction()
	{
	}
}
