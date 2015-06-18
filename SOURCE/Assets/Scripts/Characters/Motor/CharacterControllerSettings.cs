using UnityEngine;

[System.Serializable]
public class CharacterSettings
{
	[Header("RUN/WALK SETTINGS")]
	public float maxRunSpeed = 5.0f;

	[Header("AIR SETTINGS")]
	public bool enableAirControl = false;
	[Range(0.0f, 1.0f)]
	public float airSpeedRatio = 1.0f;

	[Header("LAND SETTINGS")]
	public bool stopMovementOnLand = false;
	public float landHorizontalForceToRoll = 0.55f;
	public float interruptForceOnLand = -0.3f;

	[Header("JUMP SETTINGS")]
	public float jumpForce = 11.0f;

	[Header("WALL JUMP SETTINGS")]
	public LayerMask wallJumpLayers;
	public float maxDistanceFromWall = 0.85f;
	public float wallJumpHorizontalForce = 9.0f;
	public float wallJumpVerticalForce = 12.0f;

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

	[Header("Interaction Settings")]
	public LayerMask interactiveLayer;
	public float interactionDistance = 1.0f;
}
