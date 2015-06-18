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
	private const string despawnFXName = "SkullProjectile_DestroyFX_Pool";

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
		ResetProjectile ();
	}

	public void Despawn ()
	{
		DespawnFXPool.Spawn<ParticlePoolObject> (cachedTransform.position, Quaternion.identity);
		myPool.DespawnIn(cachedGameObject, 1.5f);

		OnStartDestroy ();
	}

	public void DespawnIn (float fDelay)
	{
	}

	public void OnDespawn ()
	{
	}
	#endregion

	private void ResetProjectile()
	{
		foreach(GameObject go in goRenderers)
			go.SetActive (true);

		foreach (ParticleSystem particle in trailFX)
			particle.enableEmission = true;

		cachedRigidbody.isKinematic = false;
		cachedCollider.enabled = true;
	}

	private void OnStartDestroy()
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
