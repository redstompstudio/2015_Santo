using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundInfo
{
	public bool isGrounded;

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

	protected bool isKinematic;
	protected bool useGravity;

	public GroundInfo groundInfos = new GroundInfo();
	public MotorSettings motorSettings = new MotorSettings();

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

	public virtual float MaxRunSpeed
	{
		get{
			return motorSettings.maxRunSpeed;
		}
	}

	public virtual float JumpForce{
		get{
			return motorSettings.jumpForce;
		}
	}

	public bool IsGrounded{
		get{return groundInfos.isGrounded;}
	}

	public Transform Transform{
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return cachedTransform;
		}
	}
	#endregion

	protected virtual void Awake()	
	{
		if (groundInfos == null)
			groundInfos = new GroundInfo ();	
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

	public virtual Vector3 GetRollVelocity()
	{
		Vector3 velocity = Velocity;

//		if (Mathf.Abs (velocity.x) < motorSettings.minRollSpeed)
//			velocity.x = motorSettings.minRollSpeed * Mathf.Sign (velocity.x);

		return velocity;
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for(int i = 0; i < groundCheckers.Length; i++)
			Gizmos.DrawRay (groundCheckers [i].position, -Transform.up * groundCheckDistance);
	}
	#endif
}
