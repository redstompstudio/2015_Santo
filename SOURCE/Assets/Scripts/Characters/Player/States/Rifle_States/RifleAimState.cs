﻿using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class RifleAimState : SKMecanimState<PlayerCharacterController> 
{
	private RifleAim_Behaviour rifleAimBehaviour;

	private bool isAiming;
	private Vector3 aimPoint;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.SetVelocity (Vector3.zero);

		if (rifleAimBehaviour == null)
			rifleAimBehaviour = _machine.animator.GetBehaviour<RifleAim_Behaviour> ();

		rifleAimBehaviour.onStateEnterCallback += OnStateEnterRifleAim;
		rifleAimBehaviour.onStateIKCallback += OnStateIKRifleAim;
		rifleAimBehaviour.onStateExitCallback += OnStateExitRifleAim;

		CrossFade ("Rifle_Aim", 0.03f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		isAiming = Input.GetKey (KeyCode.Mouse1);

		if(isAiming)
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))			//FIRE!
			{
				_machine.changeState<RifleFireState>();
				return;
			}

			UpdateAimPoint();
		}
		else
		{
			_machine.changeState<IdleState> ();	
		}
	}

	#region BEHAVIOUR RIFLE_AIM
	private void OnStateEnterRifleAim()
	{
		isAiming = true;
	}

	public void OnStateIKRifleAim()
	{
		_machine.animator.SetLookAtWeight (1.0f, 1.0f, 1.0f);
		_machine.animator.SetLookAtPosition (aimPoint);
	}

	public void OnStateExitRifleAim()
	{
		isAiming = false;
	}
	#endregion

	public void UpdateAimPoint()
	{
		Vector3 pointerPosition = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay (pointerPosition);
		ray.origin = new Vector3 (ray.origin.x, ray.origin.y, context.Position.z);

		Vector3 dir = ray.direction * 100.0f;
		dir.z = context.Position.z;

		aimPoint = context.Position + dir;
		Debug.DrawRay (ray.origin, dir, Color.green, Time.deltaTime);
	}
}
