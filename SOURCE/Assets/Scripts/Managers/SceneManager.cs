using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour 
{
	private static SceneManager instance;

	private BaseCamera mainCamera;
	private PlayerCharacterController player;

	#region PROPERTIES
	public BaseCamera MainCamera{
		get{
			if (mainCamera == null)
				mainCamera = Camera.main.GetComponent<BaseCamera>();
			
			return mainCamera;
		}
	}

	public PlayerCharacterController Player{
		get{
			if (player == null)
				player = FindObjectOfType (typeof(PlayerCharacterController)) as PlayerCharacterController;

			return player;
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

	private void Awake()
	{
		if (player == null)
			player = FindObjectOfType (typeof(PlayerCharacterController)) as PlayerCharacterController;
	}

	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();
		
		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevelName);
	}

	public void OnPlayerDeath()
	{
		StartCoroutine (RespawnPlayer ());	
	}

	private IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds (2.0f);

		player.Reset ();
	}

	public void StopTime()
	{
		Time.timeScale = 0.0f;
	}

	public void ResumeTime()
	{
		Time.timeScale = 1.0f;
	}
}
