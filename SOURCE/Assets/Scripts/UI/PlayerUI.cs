using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{
	private static PlayerUI instance;

	public Image playerLifeBar;
	public Image playerInsanityBar;

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
		playerLifeBar.fillAmount = ValuesMapping.Map (pCurrent, 0.0f, pMax, 0.0f, 1.0f);
	}

	public void UpdateInsanityBar(int pMax, int pCurrent)
	{
		playerInsanityBar.fillAmount = ValuesMapping.Map (pCurrent, 0.0f, pMax, 0.0f, 1.0f);
	}
}
