using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundInfo
{
	public bool isGrounded;
	public bool isTouchingCeiling;

	public Collider groundCollider;
	public Vector3 positionOnGround;
	public Vector3 groundNormal;

	public GroundInfo(){}
}

[System.Serializable]
public class MotorSettings
{
	public float maxRunSpeed = 10.0f;
	public float jumpForce = 5.0f;

	public float minRollSpeed = 10.0f;
	public float rollSpeedMultiplier = 1.4f;

	public MotorSettings(){}
}

public class BaseCharacterMotor : MonoBehaviour 
{
	protected Transform cachedTransform;
	protected BoxCollider cachedCollider;

	private Vector3 initialColliderCenter;
	private Vector3 initialColliderSize;

	protected bool isKinematic;
	protected bool useGravity;

	public GroundInfo groundInfos = new GroundInfo();

	public LayerMask groundLayers;
	public Transform[] groundCheckers;
	public float groundCheckDistance;

	#region PROPERTIES
	public virtual bool IsKinematic
	{
		get{
			return isKinematic;
		}
		set{
			isKinematic = value;
		}
	}

	public virtual bool UseGravity{
		get{
			return useGravity;
		}
		set{
			useGravity = value;
		}
	}

	public virtual Vector3 Velocity {
		get;
		private set;
	}

	public bool IsGrounded{
		get{return groundInfos.isGrounded;}
	}

	public Transform CachedTransform{
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return cachedTransform;
		}
	}

	public BoxCollider CachedCollider{
		get{
			if (cachedCollider == null)
				cachedCollider = GetComponent<BoxCollider> ();
			return cachedCollider;
		}
	}

	public Vector3 InitialColliderSize{
		get{
			return initialColliderSize;
		}
	}

	public Vector3 InitialColliderCenter{
		get{
			return initialColliderCenter;
		}
	}
	#endregion

	protected virtual void Awake()	
	{
		if (groundInfos == null)
			groundInfos = new GroundInfo ();	

		initialColliderCenter = CachedCollider.center;
		initialColliderSize = CachedCollider.size;
	}

	protected virtual void Update()
	{
		CheckGround ();
	}
		
	public virtual bool CheckGround()
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

	public virtual void Move(Vector3 pVelocity, float pSpeed)
	{
	}

	public virtual void RotateToDirection(Vector3 pDirection)
	{
	}

	public virtual void RotateToVelocityDirection(float pSpeed, bool pIgnoreY = true)
	{
	}

	public virtual void SetVelocity(Vector3 pVelocity)
	{
	}

	public virtual void AddVelocity(Vector3 pVelocity)
	{
	}

	public virtual void ApplyForce (Vector3 pVelocity, ForceMode pMode)
	{
	}

	public virtual void StopMovement(bool pHorizontal, bool pVertical)
	{
		Vector3 velocity = Velocity;

		if (pHorizontal)
			velocity.x = 0.0f;
		if (pVertical)
			velocity.y = 0.0f;

		SetVelocity (velocity);
	}

	public bool IsTouchingCeiling()
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

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for(int i = 0; i < groundCheckers.Length; i++)
			Gizmos.DrawRay (groundCheckers [i].position, -CachedTransform.up * groundCheckDistance);
	}
	#endif

	public void ResetColliderValues()
	{
		CachedCollider.size = InitialColliderSize;
		CachedCollider.center = InitialColliderCenter;
	}

	public void ResizeCollider(Vector3 pSize)
	{
		//CachedCollider.center = pCenter;
		CachedCollider.size = pSize;

		Vector3 center = CachedCollider.center;
		center.y = CachedCollider.size.y / 2.0f;

		CachedCollider.center = center;
	}
}
