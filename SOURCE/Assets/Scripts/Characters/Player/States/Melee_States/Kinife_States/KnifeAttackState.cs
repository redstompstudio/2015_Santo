using UnityEngine;
using System.Collections;
using Prime31.StateKit;

public class KnifeAttackState : SKMecanimState<PlayerCharacterController>   
{
	private Knife_UpperSlash_Behaviour upperSlashBehaviour;
	private Knife_HorizontalSlash_Behaviour horSlashBehaviour;
	private Knife_HorizontalSlashAir_Behaviour horSlashAirBehaviour;

	private bool executeNextAttack;
	private BaseWeapon knife;

	public override void begin ()
	{
		base.begin ();

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = false;
		context.CharacterMotor.UseGravity = true;

		if (knife == null)
			knife = context.attackController.GetWeapon (WEAPON_NAME.KNIFE_SHORT);
		
		context.attackController.EquipWeapon (knife.weaponName);

		if (upperSlashBehaviour == null) 
		{
			upperSlashBehaviour = _machine.animator.GetBehaviour<Knife_UpperSlash_Behaviour> ();

			upperSlashBehaviour.onStateEnterCallback = OnStateEnterUpperSlash;
			upperSlashBehaviour.onStateUpdateCallback = OnStateUpdateUpperSlash;
			upperSlashBehaviour.onStateExitCallback = OnStateExitUpperSlash;
		}

		if (horSlashBehaviour == null) 
		{
			horSlashBehaviour = _machine.animator.GetBehaviour<Knife_HorizontalSlash_Behaviour> ();

			horSlashBehaviour.onStateEnterCallback = OnStateEnterHorSlash;
			horSlashBehaviour.onStateUpdateCallback = OnStateUpdateHorSlash;
			horSlashBehaviour.onStateExitCallback = OnStateExitHorSlash;
		}

		if(horSlashAirBehaviour == null)
		{
			horSlashAirBehaviour = _machine.animator.GetBehaviour<Knife_HorizontalSlashAir_Behaviour>();
				
			horSlashAirBehaviour.onStateEnterCallback = OnStateEnterHorSlashAir;
			horSlashAirBehaviour.onStateUpdateCallback = OnStateUpdateHorSlashAir;
			horSlashAirBehaviour.onStateExitCallback = OnStateExitHorSlashAir;
		}

		if (context.CharacterMotor.IsGrounded) 
		{
			CrossFade ("Knife_Attack_01", 0.01f, 0.0f);
		} 
		else 
		{
			Vector3 velocity = context.CharacterMotor.Velocity;

			if(velocity.y <= 0.0f)
			{
				context.CharacterMotor.UseGravity = false;
				velocity.y = 0.0f;
			}
		
			context.CharacterMotor.SetVelocity (velocity);
			CrossFade ("Knife_AirAttack_01", 0.03f, 0.0f);
		}
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}

	#region STATE_BEHAVIOUR ATTACKS
	private void OnStateEnterUpperSlash()
	{
		executeNextAttack = false;
	}

	private void OnStateUpdateUpperSlash ()
	{
		executeNextAttack |= Input.GetKeyDown (KeyCode.Mouse0);
	}

	private void OnStateExitUpperSlash()
	{
		if (executeNextAttack)
			CrossFade ("Knife_Attack_02", 0.03f, 0.0f);
		else 
		{
			context.attackController.UnequipWeapon ();
			_machine.changeState<IdleState> ();
		}
	}

	private void OnStateEnterHorSlash()
	{
		executeNextAttack = false;
	}

	private void OnStateUpdateHorSlash()
	{
		executeNextAttack |= Input.GetKeyDown (KeyCode.Mouse0);
	}

	private void OnStateExitHorSlash()
	{
		if (executeNextAttack)
			CrossFade ("Knife_Attack_01", 0.03f, 0.0f);
		else 
		{
			context.attackController.UnequipWeapon ();
			_machine.changeState<IdleState> ();
		}
	}

	private void OnStateEnterHorSlashAir ()
	{
		executeNextAttack = false;
	}

	private void OnStateUpdateHorSlashAir ()
	{
	}

	private void OnStateExitHorSlashAir ()
	{
		context.CharacterMotor.UseGravity = true;
		context.attackController.UnequipWeapon ();

		if(context.CharacterMotor.IsGrounded)
		{
			_machine.changeState<IdleState> ();
		}
		else
		{
			_machine.changeState<OnAirState> ();	
		}
	}
	#endregion
}
