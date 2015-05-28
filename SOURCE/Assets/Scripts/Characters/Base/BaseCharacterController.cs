using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseCharacterMotor))]
public class BaseCharacterController : MonoBehaviour 
{
	private Transform cachedTransform;

	[SerializeField]
	private BaseHealth health;
	private BaseCharacterMotor characterMotor;

	private Vector3 charCenterPoint;

	[SerializeField]
	private CharacterSettings gamePlaySettings;

	#region PROPERTIES
	public BaseHealth Health
	{
		get{
			if (health == null)
				health = new BaseHealth ();
			return health;
		}
	}

	public BaseCharacterMotor CharacterMotor{
		get{
			if (characterMotor == null)
				characterMotor = GetComponent<BaseCharacterMotor> ();
			return characterMotor;
		}
	}

	public CharacterSettings CharacterSettings{
		get{
			if (gamePlaySettings == null)
				gamePlaySettings = new CharacterSettings ();
			return gamePlaySettings;
		}
	}

	public Transform CachedTransform {
		get{
			if (cachedTransform == null)
				cachedTransform = transform;
			return cachedTransform;
		}
	}

	public Vector3 Position{
		get{return CachedTransform.position;}
	}

	public Quaternion Rotation{
		get{return CachedTransform.rotation;}
	}

	public Vector3 LocalScale{
		get{return CachedTransform.localScale;}
	}

	public Vector3 Forward{
		get{return CachedTransform.forward;}
	}

	public Vector3 CharCenterPoint{
		get{
			charCenterPoint = CachedTransform.position + Vector3.up;
			return charCenterPoint;
		}
	}

	#endregion

	protected virtual void Awake()
	{
		if (characterMotor == null)
			characterMotor = GetComponent<BaseCharacterMotor> ();
	}

	protected virtual void Start()
	{
	}
}
