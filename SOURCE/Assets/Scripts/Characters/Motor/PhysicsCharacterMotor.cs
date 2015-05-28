using UnityEngine;
using System.Collections;

public class PhysicsCharacterMotor : BaseCharacterMotor
{
	private Rigidbody cachedRigidbody;

	#region PROPERTIES
	public override bool IsKinematic
	{
		get{
			return cachedRigidbody.isKinematic;
		}
		set{
			cachedRigidbody.isKinematic = value;
		}
	}

	public override bool UseGravity{
		get{
			return cachedRigidbody.useGravity;
		}
		set{
			cachedRigidbody.useGravity = value;
		}
	}

	public override Vector3 Velocity {
		get {
			return cachedRigidbody.velocity;
		}
	}
	#endregion

	protected override void Awake ()
	{
		base.Awake ();

		if (cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody> ();
	}

	public override void Move (Vector3 pVelocity, float pSpeed)
	{
		base.Move (pVelocity, pSpeed);

		Vector3 velocity = cachedRigidbody.velocity;
		velocity.x = pVelocity.x * pSpeed;

		cachedRigidbody.velocity = velocity;
	}

	public override void RotateToDirection (Vector3 pDirection)
	{
		base.RotateToDirection (pDirection);

		cachedRigidbody.rotation = Quaternion.LookRotation (pDirection);
	}

	public override void RotateToVelocityDirection(float pSpeed, bool pIgnoreY = true)
	{
		Vector3 direction = cachedRigidbody.velocity;

		if (pIgnoreY)
			direction.y = 0.0f;

		if(direction != Vector3.zero)
			cachedRigidbody.rotation = Quaternion.Lerp (cachedRigidbody.rotation,
														Quaternion.LookRotation (direction), Time.deltaTime * pSpeed);
	}

	public override void SetVelocity (Vector3 pVelocity)
	{
		base.SetVelocity (pVelocity);
		cachedRigidbody.velocity = pVelocity;
	}

	public override void AddVelocity (Vector3 pVelocity)
	{
		base.AddVelocity (pVelocity);

		Vector3 velocity = cachedRigidbody.velocity + pVelocity;
		cachedRigidbody.velocity = velocity;
	}

	public override void ApplyForce (Vector3 pVelocity, ForceMode pMode)
	{
		base.ApplyForce (pVelocity, pMode);

		cachedRigidbody.AddForce (pVelocity);
	}
}
