using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class BowArrowState :  SKMecanimState<PlayerCharacterController>
{
	public BowAim_Behaviour bowAimBehaviour;

	private bool isAiming;
	private Vector3 aimPoint;

	private BaseWeapon bowArrow;

	public override void begin ()
	{
		base.begin ();

		if(bowAimBehaviour == null)
		{
			bowAimBehaviour = _machine.animator.GetBehaviour<BowAim_Behaviour> ();

			bowAimBehaviour.onStateEnterCallback += OnStateEnterBowAim;
			bowAimBehaviour.onStateIKCallback += OnStateIKBowAim;
			bowAimBehaviour.onStateExitCallback+= OnStateExitBowAim;
		}

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;
		context.CharacterMotor.SetVelocity (Vector3.zero);

		if (bowArrow == null)
			bowArrow = context.attackController.GetWeapon (WEAPON_NAME.BOW_ARROW_BASIC);

		context.attackController.EquipWeapon (bowArrow.weaponName);
		CrossFade ("Bow_Aim", 0.03f, 0.0f);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
		isAiming = Input.GetKey (KeyCode.Mouse1);

		if(isAiming)
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))			//FIRE!
			{
				_machine.changeState<BowArrowFireState>();
				return;
			}

			UpdateAimPoint();
		}
		else
		{
			context.attackController.UnequipWeapon ();
			_machine.changeState<IdleState> ();	
		}
	}

	#region BEHAVIOUR BOW_AIM
	public void OnStateEnterBowAim()
	{
		isAiming = true;
	}

	public void OnStateIKBowAim()
	{
		_machine.animator.SetLookAtWeight (1.0f, 1.0f, 1.0f);
		_machine.animator.SetLookAtPosition (aimPoint);
	}

	public void OnStateExitBowAim()
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
