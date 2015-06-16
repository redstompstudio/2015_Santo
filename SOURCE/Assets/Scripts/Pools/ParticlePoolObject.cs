using System;
using UnityEngine;
using System.Collections;

public class ParticlePoolObject : MonoBehaviour, IPoolObject
{
	public bool m_autoDespawn;			// If the particle will call the despawn function afted X seconds.
	public float m_despawnDelay;		

	public float m_duration;

	private SpawnPool m_myPool;
	
	public ParticleSystem m_particleSystem;

    public void Awake()
    {
		if(m_particleSystem == null)
		{
			m_particleSystem = GetComponent<ParticleSystem>();

	        if (m_particleSystem == null)
	            m_particleSystem = transform.GetComponentInChildren<ParticleSystem>();
		}
    }
	
	private IEnumerator DespawnInCoroutine(float fSeconds)
	{
		yield return new WaitForSeconds(fSeconds);

		if(m_particleSystem != null)
		{
			m_particleSystem.Stop(true);
			yield return new WaitForSeconds(m_particleSystem.duration);
		}
		else
		{
			yield return new WaitForSeconds(m_duration);
		}

		Despawn();
	}

	public void OnSpawn(SpawnPool pMyPool)
	{
		m_myPool = pMyPool;

		if(m_autoDespawn)
			StartCoroutine(DespawnInCoroutine(m_despawnDelay));
	}

	public void DespawnIn(float fSeconds)
	{
		if(gameObject.activeSelf)
			StartCoroutine(DespawnInCoroutine(fSeconds));
	}

	public void DespawnImmediately()
	{
		if(m_particleSystem != null)
			m_particleSystem.Stop();

		Despawn();
	}

	public void StopAndDespawn()
	{
		Debug.Log("Call Stop and Despawn");
		StartCoroutine(DespawnInCoroutine(m_despawnDelay));
	}

	public void Despawn()
	{
		if(m_myPool != null)
		{
			m_myPool.Despawn(gameObject);
		}
		else
		{
			Debug.LogError("Can't Despawn " + gameObject.name + " because the Pool is null");
		}
	}

	public void OnDespawn()
	{
	}
}
