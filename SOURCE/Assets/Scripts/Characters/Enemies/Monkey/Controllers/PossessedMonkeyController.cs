using UnityEngine;
using Prime31.StateKit;

public class PossessedMonkeyController : BaseEnemy
{
	private SKMecanimStateMachine<PossessedMonkeyController> stateMachine;

	private SpawnPool lightDamageFXPool;
	public string lightDamageFXName = "Santo_DamageLight_FX_Pool";

	private SpawnPool onKillFXPool;
	public string onKillFXName = "Monkey_Killed_FX_Pool";

	#region PROPERTIES
	protected SpawnPool LightDamageFXPool{
		get{
			if (lightDamageFXPool == null)
				lightDamageFXPool = PoolManager.Instance.GetPool (lightDamageFXName);

			return lightDamageFXPool;
		}
	}

	protected SpawnPool OnKillFXPool{
		get{
			if (onKillFXPool == null)
				onKillFXPool = PoolManager.Instance.GetPool (onKillFXName);

			return onKillFXPool;
		}
	}
	#endregion

	protected override void Start ()
	{
		base.Start ();

		stateMachine = new SKMecanimStateMachine<PossessedMonkeyController>( GetComponent<Animator>(), this, new MonkeyScoutState() );

		stateMachine.addState (new MonkeyScoutState ());
		stateMachine.addState (new MonkeyIdleState());
		stateMachine.addState (new MonkeyChaseState ());
		stateMachine.addState (new MonkeyMeleeAttackState ());
	}

	void Update()
	{
		stateMachine.update (Time.deltaTime);
	}

	public override void ReceiveDamage (BaseActor pCauser, int pDamage, DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		LightDamageFXPool.Spawn<ParticlePoolObject>(pPosition, Quaternion.identity);
		base.ReceiveDamage (pCauser, pDamage, pDamageType, pPosition);
	}

	public override void Kill ()
	{
		base.Kill ();

		OnKillFXPool.Spawn<ParticlePoolObject> (Position, Quaternion.identity);
		CachedGameObject.SetActive (false);
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (stateMachine != null)
			stateMachine.OnGizmos ();
	}
#endif
}
