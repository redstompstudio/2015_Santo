using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class MonkeyScoutState : SKMecanimState<BaseEnemy>
{
	private int curScoutPointIndex;
	private Transform[] scoutPoints;

	public override void begin ()
	{
		base.begin ();

		if(scoutPoints == null)
			scoutPoints = context.aiController.scoutPoints;
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		float distanceFromTarget = Vector3.Distance (context.Position, scoutPoints [curScoutPointIndex].position);

		if(distanceFromTarget > 1.5f)
		{
			Vector3 direction = (scoutPoints[curScoutPointIndex].position - context.Position).normalized;
			Debug.DrawRay (context.Position, direction, Color.gray);

			context.CharacterMotor.Move (direction, deltaTime * context.CharacterSettings.maxRunSpeed);
			context.CharacterMotor.RotateToDirection (direction, true);
		}
	}

	public override void OnGizmos ()
	{
		base.OnGizmos ();

		if (scoutPoints != null) {
			foreach (Transform p in scoutPoints) {
				Gizmos.DrawWireSphere (p.position, 0.5f);	
			}
		}
	}
}