using UnityEngine;
using System.Collections;

[System.Serializable]
public class ClimbMechanicsSettings 
{
	public LayerMask objectsMasks;
	public bool waitForInputToClimb;

	[Header("MATCH TARGET SETTINGS")]
	public MatchTargetWeightMask matchTargetWeightMask;
	public float matchTargetStartNormalizedTime = 0.75f;
	public float matchTargetNormalizedTime = 1.0f;

	public ClimbMechanicsSettings()
	{
	}
}
