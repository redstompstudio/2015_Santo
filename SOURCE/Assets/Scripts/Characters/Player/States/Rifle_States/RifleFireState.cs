using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class RifleFireState : SKMecanimState<PlayerCharacterController>  
{
	private RifleFire_Behaviour rifleFireBehaviour;
	private BaseCamera mainCamera;
	private Vector3 stateEnterAimPoint;

	public override void begin ()
	{
		base.begin ();

		if(rifleFireBehaviour == null)
		{
			rifleFireBehaviour = _machine.animator.GetBehaviour<RifleFire_Behaviour> ();

			rifleFireBehaviour.onStateEnterCallback += OnStateEnterRifleFire;
			rifleFireBehaviour.onStateIKCallback += OnStateIKRifleFire;
			rifleFireBehaviour.onStateExitCallback += OnStateExitRifleFire;
		}

		if (mainCamera == null)
			mainCamera = SceneManager.Instance.MainCamera;

		stateEnterAimPoint = Input.mousePosition;
		//CrossFade ("Rifle_Fire", 0.03f, 0.0f);
		CrossFade ("Rifle_Shoot", 0.03f, 0.0f, 1);
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	#region BEHAVIOUR RIFLE_FIRE
	public void OnStateEnterRifleFire()
	{
		context.attackController.GetWeapon (WEAPON_NAME.RIFLE_BASIC).Attack ();
	}

	public void OnStateIKRifleFire()
	{
		_machine.animator.SetLookAtWeight (1.0f, 1.0f, 1.0f);
		_machine.animator.SetLookAtPosition (GetAimPoint());
	}

	public void OnStateExitRifleFire()
	{
		if(Input.GetKey(KeyCode.Mouse1))
		{
			_machine.changeState<RifleAimState>();
			return;
		}
		else
		{
			context.attackController.UnequipWeapon ();
			_machine.changeState<IdleState> ();
			return;
		}
	}
	#endregion

	public Vector3 GetAimPoint()
	{
		Vector3 position = Input.mousePosition;
		position.z = -mainCamera.positionOffset.z;
		return Camera.main.ScreenToWorldPoint(position);
	}
}
