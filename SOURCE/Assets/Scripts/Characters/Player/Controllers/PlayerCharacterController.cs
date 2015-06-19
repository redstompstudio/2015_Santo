using UnityEngine;
using Prime31.StateKit;

public class PlayerCharacterController : BaseCharacterController 
{
	private SKMecanimStateMachine<PlayerCharacterController> stateMachine;

	private SpawnPool lightDamageFXPool;
	private SpawnPool heavyDamageFXPool;

	public string lightDamageFXName = "Santo_DamageLight_FX_Pool";
	public string heavyDamageFXName = "Santo_DamageHeavy_FX_Pool";

	[HideInInspector]
	public AttackController attackController;

	#region PROPERTIES
	protected SpawnPool LightDamageFXPool{
		get{
			if (lightDamageFXPool == null)
				lightDamageFXPool = PoolManager.Instance.GetPool (lightDamageFXName);

			return lightDamageFXPool;
		}
	}

	protected SpawnPool HeavyDamageFXPool{
		get{
			if (heavyDamageFXPool == null)
				heavyDamageFXPool = PoolManager.Instance.GetPool (heavyDamageFXName);

			return heavyDamageFXPool;
		}
	}
	#endregion

	protected override void Awake ()
	{
		base.Awake ();

		if (attackController == null)
			attackController = GetComponent<AttackController> ();
	}

	protected override void Start ()
	{
		base.Start ();

		stateMachine = new SKMecanimStateMachine<PlayerCharacterController>( GetComponent<Animator>(), this, new IdleState() );

		stateMachine.addState (new DeadState ());
		stateMachine.addState (new WalkState());
		stateMachine.addState (new JumpState ());
		stateMachine.addState (new OnAirState ());
		stateMachine.addState (new LandState ());

		stateMachine.addState (new CrouchState ());
		stateMachine.addState (new WallJumpState ());
		stateMachine.addState (new RollState ());
		stateMachine.addState (new DashBackState ());
		stateMachine.addState (new SlideState ());
		stateMachine.addState (new GrabLedgeState ());

		stateMachine.addState (new RifleAimState ());
		stateMachine.addState (new RifleFireState ());

		stateMachine.addState (new KnifeAttackState());

		stateMachine.addState (new PullGroundLeverState ());

#if UNITY_EDITOR
		stateMachine.onStateChanged += () =>
		{
			
		};
#endif	
	}

	void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.J))
			Debug.Break ();

		if(Input.GetKeyDown(KeyCode.F5))
			XMLSerializer.Save<CharacterSettings>(CharacterSettings, "Player_CharSettings.xml");
		else if(Input.GetKeyDown(KeyCode.F9))
			CharacterSettings = XMLSerializer.Load<CharacterSettings> ("Player_CharSettings.xml");
#endif

		if(Input.GetKeyDown(KeyCode.H))
			Reset ();

		stateMachine.update( Time.deltaTime );
	}

	void FixedUpdate()
	{
		stateMachine.fixedUpdate (Time.deltaTime);
	}

	public override void Reset ()
	{
		base.Reset ();

		stateMachine.changeState<IdleState> ();
		CachedTransform.position = CheckpointManager.Instance.CurrentCheckpoint.transform.position;
	}

	public override void ReceiveDamage (BaseActor pCauser, int pDamage, DAMAGE_TYPE pDamageType, Vector3 pPosition)
	{
		int lightDamageBase = (int)(Health.MaxHealth * 0.1f);
		int mediumDamageBase = (int)(Health.MaxHealth * 0.3f);
		int highDamageBase = (int)(Health.MaxHealth);

		if(pDamage <= lightDamageBase)
			LightDamageFXPool.Spawn<ParticlePoolObject>(pPosition, Quaternion.identity);
		else if(pDamage <= mediumDamageBase)
			HeavyDamageFXPool.Spawn<ParticlePoolObject>(pPosition, Quaternion.identity);
		else if(pDamage <= highDamageBase)
			HeavyDamageFXPool.Spawn<ParticlePoolObject>(pPosition, Quaternion.identity);

		base.ReceiveDamage (pCauser, pDamage, pDamageType, pPosition);

		if(PlayerUI.Instance != null)
			PlayerUI.Instance.UpdateLifeBar (Health.MaxHealth, Health.CurrentHealth);
	}

	public override void Kill ()
	{
		base.Kill ();

		stateMachine.changeState<DeadState> ();
		SceneManager.Instance.OnPlayerDeath();

		CachedGameObject.SetActive (false);
	}

#if UNITY_EDITOR
	void OnGUI()
	{
		if(stateMachine != null)
			GUILayout.Box (stateMachine.currentState.ToString());
	}

	void OnDrawGizmos()
	{
		if(stateMachine != null)
			stateMachine.OnGizmos ();
	}
#endif
}
