using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPoolObject
{
	void OnSpawn(SpawnPool pMyPool);
	void Despawn();
	void DespawnIn(float fDelay);
	void OnDespawn();
	 
	/*
	public void OnSpawn(SpawnPool pMyPool)
	{
	}

	public void Despawn()
	{
	}

	public void DespawnIn(float pDelay)
	{
	}

	public void OnDespawn()
	{
	}
	*/
}

public class PoolManager : MonoBehaviour
{
	private static PoolManager m_instance;
	public Dictionary<string, SpawnPool> spawnPoolsDictionary;
	
	private static GameObject m_poolsParent;
	
	public static PoolManager Instance
	{
		get{
			if (m_instance == null) {
				m_instance = GameObject.FindObjectOfType (typeof(PoolManager)) as PoolManager;
				
				if (m_instance == null) {
					GameObject go = new GameObject ("PoolManager");
					m_instance = go.AddComponent<PoolManager> ();
				}

				m_poolsParent = m_instance.gameObject;
	            
				if (m_poolsParent == null)
					m_poolsParent = new GameObject ("Pools");
			}
		
			return m_instance;
		}
	}

	public void RegisterPool(string sName, SpawnPool pPool)
	{
		if(spawnPoolsDictionary == null)
			spawnPoolsDictionary = new Dictionary<string, SpawnPool>();
		
		if(spawnPoolsDictionary.ContainsKey(sName))
		{
		//	Debug.Log("Pool name " + sName + " already in use");
		}
		else
			spawnPoolsDictionary.Add(sName, pPool);
	}
	
	public SpawnPool GetPool(string sName)
	{
		if(spawnPoolsDictionary == null)
		{
			spawnPoolsDictionary = new Dictionary<string, SpawnPool>();
			return null;
		}
		else if(spawnPoolsDictionary.ContainsKey(sName))
			return spawnPoolsDictionary[sName];
		else
			return null;
	}
	
	public static SpawnPool CreatePool(GameObject pPrefab, string pName, int pInstances)
	{
		if(pPrefab != null)
		{
			GameObject newPool = new GameObject(pName);
			SpawnPool pool = newPool.AddComponent<SpawnPool>();
			pool.Initialize(pPrefab, pName, pInstances);
			newPool.transform.parent = m_poolsParent.transform;
			return pool;
		}
		else
		{
			Debug.LogError("Could not create pool - " + pName + " because the prefab is null.");
			return null;
		}
	}
	
	public void ClearInstance()
	{
		m_instance = null;
		spawnPoolsDictionary = null;
		m_poolsParent = null;
	}
}
