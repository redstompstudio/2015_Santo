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

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class BaseCharacterMotor : MonoBehaviour 
{
	protected GameObject cachedGameObject;
	protected Transform cachedTransform;
	protected BoxCollider cachedCollider;
	protected Rigidbody cachedRigidbody;

	private Vector3 initialColliderCenter;
	private Vector3 initialColliderSize;

	protected bool isKinematic;
	protected bool useGravity;

	public GroundInfo groundInfos = new GroundInfo();

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

	public virtual bool IsGrounded{
		get{return groundInfos.isGrounded;}
	}

	public GameObject CachedGameObject{
		get{
			if (cachedGameObject == null)
				cachedGameObject = gameObject;
			return cachedGameObject;
		}
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

	public Rigidbody CachedRigidbody{
		get{
			if (cachedRigidbody == null)
				cachedRigidbody = GetComponent<Rigidbody> ();
			return cachedRigidbody;
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
		return false;
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

	public virtual bool IsTouchingCeiling()
	{
		return false;
	}

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
