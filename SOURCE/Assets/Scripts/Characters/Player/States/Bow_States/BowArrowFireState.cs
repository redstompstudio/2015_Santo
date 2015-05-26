using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class BowArrowFireState : SKMecanimState<PlayerCharacterController>
{
	public BowFire_Behaviour bowFireBehaviour;
	private Vector3 stateEnterAimPoint;

	public override void begin ()
	{
		base.begin ();

		if(bowFireBehaviour == null)
		{
			bowFireBehaviour = _machine.animator.GetBehaviour<BowFire_Behaviour> ();

			bowFireBehaviour.onStateIKCallback += OnStateIKBowFire;
			bowFireBehaviour.onStateExitCallback += OnStateExitBowFire;
		}

		stateEnterAimPoint = Input.mousePosition;
		CrossFade ("Bow_Fire", 0.03f, 0.0f);

	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	#region BEHAVIOUR BOW_FIRE
	public void OnStateIKBowFire()
	{
		_machine.animator.SetLookAtWeight (1.0f, 1.0f, 1.0f);
		_machine.animator.SetLookAtPosition (GetAimPoint());
	}

	public void OnStateExitBowFire()
	{
		if(Input.GetKey(KeyCode.Mouse1))
		{
			//CrossFade ("Bow_Aim", 0.03f, 0.0f);
			_machine.changeState<BowArrowState>();
			return;
		}
		else
		{
			_machine.changeState<IdleState> ();
			return;
		}
	}
	#endregion

	public Vector3 GetAimPoint()
	{
		Vector3 pointerPosition = stateEnterAimPoint;

		Ray ray = Camera.main.ScreenPointToRay (pointerPosition);
		ray.origin = new Vector3 (ray.origin.x, ray.origin.y, context.Position.z);

		Vector3 dir = ray.direction * 100.0f;
		dir.z = context.Position.z;

		Debug.DrawRay (ray.origin, dir, Color.green, Time.deltaTime);
		return (context.Position + dir);	
	}
}
