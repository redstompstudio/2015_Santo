using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIController
{
	private Vector3 targetPosition;

	public Transform targetTransform;

	public Transform[] scoutPoints;

	#region PROPERTIES
	public Vector3 TargetPosition{
		get{
			targetPosition = targetTransform.position;
			return targetPosition;
		}
	}
	#endregion
}
