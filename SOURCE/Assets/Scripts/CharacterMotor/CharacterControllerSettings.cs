using UnityEngine;

[System.Serializable]
public class CharacterSettings
{
	[Header("RUN/WALK SETTINGS")]
	public float maxRunSpeed = 5.0f;

	[Header("AIR SETTINGS")]
	public bool hasAirControl = false;

	[Header("LAND SETTINGS")]
	public float interruptForceOnLand = -0.3f;

	[Header("JUMP SETTINGS")]
	public float jumpForce = 11.0f;

	[Header("WALL JUMP SETTINGS")]
	public LayerMask wallJumpLayers;

	[Header("SLIDE SETTINGS")]
	public float slideSpeed = 5.0f;
	public float slideSpeedDecreaseRate = 2.8f;

	[Header("ROLL SETTINGS")]
	public float rollSpeed = 6.0f;

	[Header("DASH BACK SETTINGS")]
	public float dashBackSpeed = 5.0f;

	[Header("CLIMB EDGE SETTINGS")]
	public LayerMask climbEdgeLayers;
	public bool waitForInputToClimb = false;
	public float matchStartTime = 0.0f;
	public float matchTargetTime = 0.95f;



}
