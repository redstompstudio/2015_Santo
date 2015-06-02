using UnityEngine;
using System.Collections;

public class PhysicsCharacterMotor : BaseCharacterMotor
{
	public LayerMask groundLayers;
	public Transform[] groundCheckers;
	public float groundCheckDistance;

	#region PROPERTIES
	public override bool IsKinematic
	{
		get{
			return CachedRigidbody.isKinematic;
		}
		set{
			CachedRigidbody.isKinematic = value;
		}
	}

	public override bool UseGravity{
		get{
			return CachedRigidbody.useGravity;
		}
		set{
			CachedRigidbody.useGravity = value;
		}
	}

	public override Vector3 Velocity {
		get {
			return CachedRigidbody.velocity;
		}
	}
	#endregion

	public override bool CheckGround()
	{
		groundInfos.isGrounded = false;

		if(groundCheckers != null)
		{
			for(int i = 0; i < groundCheckers.Length; i++)
			{
				Ray ray = new Ray (groundCheckers [i].position, Vector3.down);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, groundCheckDistance, groundLayers))
				{
					groundInfos.isGrounded = true;
					groundInfos.groundCollider = hit.collider;
					groundInfos.positionOnGround = hit.point;
					groundInfos.groundNormal = hit.normal;
				}
			}
		}

		return groundInfos.isGrounded;
	}

	public override bool IsTouchingCeiling()
	{
		if (groundCheckers != null) 
		{
			for (int i = 0; i < groundCheckers.Length; i++) 
			{
				Ray ray = new Ray (groundCheckers [i].position, CachedTransform.up);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, InitialColliderSize.y, groundLayers)) 
				{
					return true;
				}
			}
		}

		return false;
	}

	public override void Move (Vector3 pVelocity, float pSpeed)
	{
		base.Move (pVelocity, pSpeed);

		Vector3 velocity = CachedRigidbody.velocity;
		velocity.x = pVelocity.x * pSpeed;

		CachedRigidbody.velocity = velocity;
	}

	public override void RotateToDirection (Vector3 pDirection)
	{
		base.RotateToDirection (pDirection);

		CachedRigidbody.rotation = Quaternion.LookRotation (pDirection);
	}

	public override void RotateToVelocityDirection(float pSpeed, bool pIgnoreY = true)
	{
		Vector3 direction = CachedRigidbody.velocity;

		if (pIgnoreY)
			direction.y = 0.0f;

		if(direction != Vector3.zero)
			CachedRigidbody.rotation = Quaternion.Lerp (CachedRigidbody.rotation,
														Quaternion.LookRotation (direction), Time.deltaTime * pSpeed);
	}

	public override void SetVelocity (Vector3 pVelocity)
	{
		base.SetVelocity (pVelocity);
		CachedRigidbody.velocity = pVelocity;
	}

	public override void AddVelocity (Vector3 pVelocity)
	{
		base.AddVelocity (pVelocity);

		Vector3 velocity = CachedRigidbody.velocity + pVelocity;
		CachedRigidbody.velocity = velocity;
	}

	public override void ApplyForce (Vector3 pVelocity, ForceMode pMode)
	{
		base.ApplyForce (pVelocity, pMode);

		CachedRigidbody.AddForce (pVelocity);
	}


	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (groundCheckers != null) 
		{
			for (int i = 0; i < groundCheckers.Length; i++)
				Gizmos.DrawRay (groundCheckers [i].position, -CachedTransform.up * groundCheckDistance);
		}
	}
	#endif
}
