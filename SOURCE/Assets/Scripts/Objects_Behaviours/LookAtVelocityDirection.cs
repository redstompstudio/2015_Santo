using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class LookAtVelocityDirection : MonoBehaviour 
{
	private Rigidbody cachedRigidbody;

	void Awake()
	{
		if (cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody> ();

		if (cachedRigidbody.isKinematic)
			this.enabled = false;
	}

	void FixedUpdate()
	{
		if(cachedRigidbody.velocity != Vector3.zero)
			cachedRigidbody.rotation = Quaternion.LookRotation (cachedRigidbody.velocity);
	}
}
