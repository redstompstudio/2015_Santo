using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SkullBulletController : MonoBehaviour, IPoolObject
{
	private GameObject cachedGameObject;
	private Transform cachedTransform;
	private Rigidbody cachedRigidbody;
	private Collider cachedCollider;

	private SpawnPool myPool;

	private SpawnPool despawnFXPool;
	private const string despawnFXName = "SkullProjectile_DespawnFX_Pool";

	public float movementSpeed = 10.0f;

	public List<string> tagsList = new List<string>();

	#region Properties
	public SpawnPool DespawnFXPool{
		get{
			if (despawnFXPool == null)
				despawnFXPool = PoolManager.Instance.GetPool (despawnFXName);

			return despawnFXPool;
		}
	}
	#endregion

	private void Awake()
	{
		if (cachedGameObject == null)
			cachedGameObject = gameObject;

		if (cachedTransform == null)
			cachedTransform = transform;

		if(cachedRigidbody == null)
			cachedRigidbody = GetComponent<Rigidbody> ();

		if (cachedCollider == null)
			cachedCollider = GetComponent<Collider> ();

		cachedRigidbody.useGravity = false;
		cachedRigidbody.isKinematic = false;
		cachedRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

		cachedCollider.isTrigger = true;
	}

	private void FixedUpdate()
	{
		cachedRigidbody.velocity = cachedTransform.forward * movementSpeed;
	}

	void OnTriggerEnter(Collider pOther)
	{
		if(tagsList.Contains(pOther.tag))
		{
			BaseActor actor = pOther.GetComponent<BaseActor> ();

			if (actor)
				actor.Health.DoDamage (20);

			Despawn ();
		}
	}

	#region IPoolObject implementation
	public void OnSpawn (SpawnPool pMyPool)
	{
		myPool = pMyPool;
	}

	public void Despawn ()
	{
		DespawnFXPool.Spawn<ParticlePoolObject> (cachedTransform.position, Quaternion.identity);
		myPool.Despawn (cachedGameObject);
	}

	public void DespawnIn (float fDelay)
	{
	}

	public void OnDespawn ()
	{
	}
	#endregion
}
