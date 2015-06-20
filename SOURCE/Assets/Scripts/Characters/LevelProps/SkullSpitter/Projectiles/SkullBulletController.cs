using UnityEngine;
using System.Collections.Generic;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(Rigidbody))]
public class SkullBulletController : BaseActor, IPoolObject
{
	protected Rigidbody cachedRigidbody;
	protected Collider cachedCollider;
	protected SpawnPool myPool;
	protected SpawnPool despawnFXPool;

	public int damage = 20;
	public DAMAGE_TYPE damageType;

	public string despawnFXName = "SkullProjectile_DestroyFX_Pool";
	public string crashSoundName = "SkullProjectile_Crashing";

	public float movementSpeed = 10.0f;
	public List<string> tagsList = new List<string>();

	public ParticleSystem[] trailFX;
	public GameObject[] goRenderers;

	#region Properties
	public SpawnPool DespawnFXPool{
		get{
			if (despawnFXPool == null)
				despawnFXPool = PoolManager.Instance.GetPool (despawnFXName);

			return despawnFXPool;
		}
	}
	#endregion

	protected virtual void Awake()
	{
		if(cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody> ();

		if (cachedCollider == null)
			cachedCollider = GetComponent<Collider> ();

		cachedRigidbody.useGravity = false;
		cachedRigidbody.isKinematic = false;
		cachedRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

		cachedCollider.isTrigger = true;
	}

	protected virtual void FixedUpdate()
	{
		cachedRigidbody.velocity = Forward * movementSpeed;
	}

	protected virtual void OnTriggerEnter(Collider pOther)
	{
		if(tagsList.Contains(pOther.tag))
		{
			BaseActor actor = pOther.GetComponent<BaseActor> ();

			if (actor) 
				actor.ReceiveDamage (null, damage, damageType, Position);

			Despawn ();
		}
	}

	#region IPoolObject implementation
	public virtual void OnSpawn (SpawnPool pMyPool)
	{
		myPool = pMyPool;
		Reset ();
	}

	public virtual void Despawn ()
	{
		DespawnFXPool.Spawn<ParticlePoolObject> (Position, Quaternion.identity);
		myPool.DespawnIn(CachedGameObject, 1.5f);	

		OnStartDestroy ();
	}

	public virtual void DespawnIn (float fDelay)
	{
	}

	public virtual void OnDespawn ()
	{
	}
	#endregion

	public override void Kill ()
	{
		Despawn ();
	}

	public override void Reset ()
	{
		Health.currentHealth = Health.MaxHealth;

		foreach(GameObject go in goRenderers)
			go.SetActive (true);

		foreach (ParticleSystem particle in trailFX)
			particle.enableEmission = true;

		cachedRigidbody.isKinematic = false;
		cachedCollider.enabled = true;
	}

	protected virtual void OnStartDestroy()
	{
		foreach(GameObject go in goRenderers)
			go.SetActive (false);

		foreach (ParticleSystem particle in trailFX)
			particle.enableEmission = false;

		cachedRigidbody.velocity = Vector3.zero;
		cachedRigidbody.angularVelocity = Vector3.zero;
		cachedRigidbody.isKinematic = true;

		cachedCollider.enabled = false;
	}
}
