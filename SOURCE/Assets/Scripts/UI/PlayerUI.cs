using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{
	private static PlayerUI instance;

	public Slider playerLifeBar;
	public Slider playerInsanityBar;

#region PROPERTIES
	public static PlayerUI Instance{
		get{
			if (instance == null)
				instance = FindObjectOfType (typeof(PlayerUI)) as PlayerUI;

			return instance;
		}
	}
#endregion

	public void UpdateLifeBar(int pMax, int pCurrent)
	{
		playerLifeBar.value = ValuesMapping.Map (pCurrent, 0.0f, pMax, 0.0f, 1.0f);
	}

	public void UpdateInsanityBar(int pMax, int pCurrent)
	{
		playerInsanityBar.value = ValuesMapping.Map (pCurrent, 0.0f, pMax, 0.0f, 1.0f);
	}
}
