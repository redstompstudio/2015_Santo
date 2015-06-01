using UnityEngine;
using System.Collections;

public class StickOnCollision : MonoBehaviour 
{
	private Collider cachedCollider;
	private Rigidbody cachedRigidbody;

	public LayerMask ignoreCollisionLayer;

	void Awake()
	{
		cachedCollider = GetComponent<Collider> ();
		cachedRigidbody = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter(Collision pOther)
	{
		cachedRigidbody.isKinematic = true;
	}
}