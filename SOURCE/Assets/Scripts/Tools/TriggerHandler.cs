using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class TriggerHandler : MonoBehaviour 
{
	private Rigidbody cachedRigidbody;
	private Collider trigger;

	public delegate void OnTriggerEnterEvent(Collider pCollider);
	public OnTriggerEnterEvent onTriggerEnterCallback;

	public delegate void OnTriggerExitEvent(Collider pCollider);
	public OnTriggerExitEvent onTriggerExitCallback;

	public List<string> tags;

	private void Awake()
	{
		if (cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody> ();

		if (trigger == null)
			trigger = GetComponent<Collider> ();

		trigger.isTrigger = true;
		cachedRigidbody.isKinematic = true;
		cachedRigidbody.useGravity = false;
	}

	private void OnTriggerEnter(Collider pOther)
	{
		if (tags == null || tags.Count == 0 || tags.Contains(pOther.tag)) 
		{
			if (onTriggerEnterCallback != null)
				onTriggerEnterCallback (pOther);
		}
	}

	private void OnTriggerExit(Collider pOther)
	{
		if (tags == null || tags.Count == 0 || tags.Contains (pOther.tag))
		{
			if (onTriggerExitCallback != null)
				onTriggerExitCallback (pOther);
		}
	}
}
