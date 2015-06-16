using UnityEngine;
using System.Collections;
using Prime31.StateKit;

[RequireComponent(typeof(BaseCharacterMotor))]
public class BaseCharacterController : BaseActor 
{
	private BaseCharacterMotor characterMotor;
	[SerializeField]
	private CharacterSettings characterSettings;

	private Vector3 charCenterPoint;

	#region PROPERTIES
	public BaseCharacterMotor CharacterMotor{
		get{
			if (characterMotor == null)
				characterMotor = GetComponent<BaseCharacterMotor> ();
			return characterMotor;
		}
	}

	public CharacterSettings CharacterSettings {
		get {
			if (characterSettings == null)
				characterSettings = new CharacterSettings ();
			return characterSettings;
		}

		protected set{
			characterSettings = value;
		}
	}

	public Vector3 CharCenterPoint{
		get{
			Vector3 offset = CharacterMotor.CurrentColliderSize / 2.0f;
			offset.x = offset.z = 0.0f;

			charCenterPoint = CachedTransform.position + offset;
			return charCenterPoint;
		}
	}

	#endregion

	protected override void Awake()
	{
	}

	protected override void Start()
	{
	}
}
