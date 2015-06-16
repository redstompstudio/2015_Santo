using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn an object of the pool 'poolName' that will last 'X' seconds.
/// </summary>
public class PoolSpawner : MonoBehaviour 
{
	public string poolName;
	private SpawnPool pool;

	public Transform spawnPositionRef;
	public float instanceLifeTime = 4.0f;
	
	public float minCooldown = 1.0f;
	public float maxCooldown = 3.0f;
	private float curCooldown = 0.0f;
	
	private float curWaitingTime = 0.0f;
	
	void Start()
	{
		if(spawnPositionRef == null)
			spawnPositionRef = transform;
		
		curWaitingTime = maxCooldown;
	}
	
	public void Update()
	{
		if(curWaitingTime >= curCooldown)
		{
			GameObject go = GetSpawnPool().Spawn(spawnPositionRef.position, spawnPositionRef.rotation);
			pool.DespawnIn(go, instanceLifeTime);

			curWaitingTime = 0.0f;
			curCooldown = Random.Range(minCooldown, maxCooldown);
		}
		else
		{
			curWaitingTime += Time.deltaTime;
		}
	}

	public SpawnPool GetSpawnPool()
	{
		if(pool == null)
			pool = PoolManager.Instance.GetPool(poolName);

		return pool;
	}
}
