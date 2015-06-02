using UnityEngine;
using System.Collections;

public class StickOnCollision : MonoBehaviour 
{
	private Rigidbody cachedRigidbody;

	public LayerMask ignoreCollisionLayer;

	void Awake()
	{
		cachedRigidbody = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision pOther)
	{
		cachedRigidbody.isKinematic = true;
	}
}