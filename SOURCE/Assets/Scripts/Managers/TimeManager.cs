using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour 
{
	private static TimeManager instance;
	private float timeScale;

	#region PROPERTIES
	public static TimeManager Instance{
		get{
			if (instance == null)
				instance = FindObjectOfType (typeof(TimeManager)) as TimeManager;
			return instance;
		}
	}

	public float DeltaTime{
		get{
			return timeScale;
		}
		set{
			timeScale = value;
			Time.timeScale = timeScale;
		}
	}
	#endregion
}
