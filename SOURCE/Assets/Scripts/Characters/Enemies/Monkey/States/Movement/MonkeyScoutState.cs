using UnityEngine;
using Prime31.StateKit;

public class MonkeyScoutState : SKMecanimState<PossessedMonkeyController>
{
	private Transform[] scoutPoints;
	private int curPointIndex = 0;

	private Vector3 pointPosition;

	public override void begin ()
	{
		base.begin ();

		if(scoutPoints == null)
			scoutPoints = context.scoutPoints;

		if(scoutPoints == null || scoutPoints.Length < 1)
		{
			_machine.changeState<MonkeyIdleState> ();
			return;
		}

		pointPosition = scoutPoints [curPointIndex].position;

		CrossFade("Walk", 0.1f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		float distance = Vector3.Distance (context.Position, pointPosition);

		if(distance <= context.stopDistance)
		{
			OnReachedPoint ();
			_machine.changeState<MonkeyIdleState> ();
			return;
		}
		else
		{
			Vector3 direction = (pointPosition - context.Position).normalized;
			direction.y = 0.0f;
			direction.z = 0.0f;

			context.CharacterMotor.Move (direction, context.CharacterSettings.maxRunSpeed * deltaTime);

			if(direction != Vector3.zero)
				context.CharacterMotor.RotateToDirection (direction);
		}
	}

	private void OnReachedPoint()
	{
		curPointIndex++;

		if (curPointIndex > scoutPoints.Length - 1)
			curPointIndex = 0;
	}
}
