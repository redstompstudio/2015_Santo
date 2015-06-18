using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPool : MonoBehaviour 
{
	public string m_poolName;					//The pool name (important to find them later)
	public GameObject m_objectPrefab;			//The pool object
	
	public bool m_createOnStart;				//Will create some instances on start?
	
	public int m_startInstances;				//Starting instances.
	public int m_extraInstances = 4;  			//If we need more objects than we have created, create more X instances.
	
	private List<GameObject> m_poolObjects;			//Objects in the pool
    private List<GameObject> m_spawnedObjects;		//Spawned objects
	
	void Awake()
	{
		if(m_poolName == "")
			m_poolName = gameObject.name;
		
		if(m_createOnStart)
			CreateInstances(m_startInstances);
	}
	
	public void Initialize(GameObject pPrefab, string pPoolName, int pStartInstances)
	{
		m_objectPrefab = pPrefab;
		m_poolName = pPoolName;
		m_startInstances = pStartInstances;
		
		CreateInstances(m_startInstances);
	}
	
	/// <summary>
	/// Instantiate the objects 
	/// </summary>
	/// <param name='fCount'>
	/// The number of new instances.
	/// </param>
	public void CreateInstances(float fCount)
	{
		if(m_poolObjects == null)
			m_poolObjects = new List<GameObject>();
		
		if(m_spawnedObjects == null)
			m_spawnedObjects = new List<GameObject>();

	    if (fCount <= 0)
	        fCount = 1;
		
		PoolManager.Instance.RegisterPool(m_poolName, this);
		
		for(int i = 0; i < fCount; i++)
		{
			GameObject newInstance = Instantiate(m_objectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newInstance.transform.parent = transform;
			newInstance.transform.localPosition = Vector3.zero;
			newInstance.SetActive(false);

			m_poolObjects.Add(newInstance);
		}
	}
	
	/// <summary>
	/// Search for an object in the pool and spawn it! Returns a GameObject!
	/// </summary>
	/// <param name='pPosition'>
	/// The position where the object will be created.
	/// </param>
	/// <param name='rRotation'>
	/// The rotation of the spawned object.
	/// </param>
	public GameObject Spawn(Vector3 pPosition, Quaternion pRotation)
	{	
		if(m_poolObjects.Count == 0)
			CreateInstances(m_extraInstances);

	    if (m_poolObjects != null && m_poolObjects.Count > 0)
	    {
	        GameObject go = m_poolObjects[0];
	        m_poolObjects.Remove(go);

	        go.transform.position = pPosition;
	        go.transform.rotation = pRotation;
            go.SetActive(true);

			IPoolObject poolObj = go.GetComponent(typeof(IPoolObject)) as IPoolObject;
			
			if(poolObj != null)
				poolObj.OnSpawn(this);

            m_spawnedObjects.Add(go);
	        return go;
	    }

		Debug.Log("No more instances in the pool.");
		return null;
	}

	/// <summary>
	/// Search for an object in the pool and spawn it! Returns a Generic!
	/// </summary>
	/// <param name='pPosition'>
	/// The position where the object will be created.
	/// </param>
	/// <param name='rRotation'>
	/// The rotation of the spawned object.
	/// </param>
	public T Spawn<T>(Vector3 pPosition, Quaternion pRotation) where T : Component
	{
		if(m_poolObjects.Count == 0)
			CreateInstances(m_extraInstances);

	    if (m_poolObjects != null && m_poolObjects.Count > 0)
	    {
	        GameObject go = m_poolObjects[0];
            m_poolObjects.Remove(go);

	        go.transform.position = pPosition;
	        go.transform.rotation = pRotation;
            go.SetActive(true);

            m_spawnedObjects.Add(go);

            T instantiated = go.GetComponent<T>();
            IPoolObject poolObj = go.GetComponent(typeof(IPoolObject)) as IPoolObject;

            if(poolObj != null)
                poolObj.OnSpawn(this);

	        return instantiated;
	    }

		Debug.Log("No more instances of " + m_objectPrefab.name + " in the pool.");
		return null;
	}
	
	/// <summary>
	/// Disable the object and put it back in the pool.
	/// </summary>
	/// <param name='pPoolObject'>
	/// The object that will be despawned.
	/// </param>
	public void Despawn(GameObject pPoolObject)
	{
	    if (m_spawnedObjects.Contains(pPoolObject))
	    {
	        m_spawnedObjects.Remove(pPoolObject);
			m_poolObjects.Insert(0, pPoolObject);

            IPoolObject poolObject = pPoolObject.GetComponent(typeof(IPoolObject)) as IPoolObject;

            if (poolObject != null)
                poolObject.OnDespawn();

			pPoolObject.transform.parent = transform;
            pPoolObject.SetActive(false);
	    }
	}
	
	private IEnumerator _Despawn(GameObject pPoolObject, float pTime)
	{
		yield return new WaitForSeconds(pTime);
		
		if(pPoolObject.activeInHierarchy && pPoolObject.activeSelf)
			Despawn(pPoolObject);
	}
	
	/// <summary>
	/// Despawn an object in X seconds
	/// </summary>
	/// <param name='pPoolObject'>
	/// The pool object.
	/// </param>
	/// <param name='pTime'>
	/// Time in seconds.
	/// </param>
	public void DespawnIn(GameObject pPoolObject, float pTime)
	{
		StartCoroutine(_Despawn(pPoolObject, pTime));
	}
	
	/// <summary>
	/// Disable the first object in the pool.
	/// </summary>
	public void DespawnFirst()
	{
        if(m_spawnedObjects != null && m_spawnedObjects.Count > 0)
            Despawn(m_spawnedObjects[0]);
	}
	
	/// <summary>
	/// Disable all the object in the pool.
	/// </summary>
	public void DespawnAll()
	{
		while(m_spawnedObjects.Count!= 0)
			Despawn( m_spawnedObjects[0]);
	}

	/// <summary>
	/// Return a list of spawned objects.
	/// </summary>
	/// <returns>The spawned objects.</returns>
	public List<GameObject> GetSpawnedObjects()
	{
		if(m_spawnedObjects == null)
			m_spawnedObjects = new List<GameObject>();

		return m_spawnedObjects;
	}
}
