using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour 
{
	private static SceneManager instance;

	private BaseCamera mainCamera;

	#region PROPERTIES
	public BaseCamera MainCamera{
		get{
			if (mainCamera == null)
				mainCamera = Camera.main.GetComponent<BaseCamera>();
			
			return mainCamera;
		}
	}
	#endregion

	public static SceneManager Instance
	{
		get{
			if (instance == null)
				instance = FindObjectOfType (typeof(SceneManager)) as SceneManager;
			return instance;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();

		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevelName);
	}
}
