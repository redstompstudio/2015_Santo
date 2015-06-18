using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class RifleAimState : SKMecanimState<PlayerCharacterController> 
{
	private RifleAim_Behaviour rifleAimBehaviour;
	private BaseCamera mainCamera;
	private bool isAiming;
	private Vector3 aimPoint;

	private BaseWeapon rifle;

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

		if (rifle == null)
			rifle = context.attackController.GetWeapon (WEAPON_NAME.RIFLE_BASIC);

		if (mainCamera == null)
			mainCamera = SceneManager.Instance.MainCamera;

		context.attackController.EquipWeapon (rifle.weaponName);
		CrossFade ("Rifle_Aim_Tree", 0.03f, 0.0f);
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
			context.attackController.UnequipWeapon ();
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
		Vector3 position = Input.mousePosition;
		position.z = -mainCamera.positionOffset.z;
		aimPoint = Camera.main.ScreenToWorldPoint(position);
	}

	public override void OnGizmos ()
	{
		base.OnGizmos ();

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (aimPoint, 0.3f);
	}
}
