using UnityEngine;

[System.Serializable]
public class CharacterSettings
{
	[Header("RUN/WALK SETTINGS")]
	public float maxRunSpeed = 5.0f;

	[Header("AIR SETTINGS")]
	public bool hasAirControl = false;

	[Header("LAND SETTINGS")]
	public float landHorizontalForceToRoll = 0.55f;
	public float interruptForceOnLand = -0.3f;

	[Header("JUMP SETTINGS")]
	public float jumpForce = 11.0f;

	[Header("WALL JUMP SETTINGS")]
	public LayerMask wallJumpLayers;

	[Header("CROUCH SETTINGS")]
	public float crouchColliderSizeY = 0.95f;

	[Header("SLIDE SETTINGS")]
	public float slideColliderSizeY = 0.95f;
	public float slideSpeed = 5.0f;
	public float slideSpeedDecreaseRate = 2.8f;

	[Header("ROLL SETTINGS")]
	public float rollColliderSizeY = 0.95f;
	public float rollSpeed = 6.0f;

	[Header("DASH BACK SETTINGS")]
	public float dashBackColliderSizeY = 0.95f;
	public float dashBackSpeed = 5.0f;

	[Header("CLIMB EDGE SETTINGS")]
	public LayerMask climbEdgeLayers;
	public bool waitForInputToClimb = false;
	public float matchStartTime = 0.0f;
	public float matchTargetTime = 0.95f;
}
