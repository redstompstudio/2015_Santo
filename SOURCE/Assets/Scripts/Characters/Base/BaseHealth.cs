using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseHealth 
{
	public float baseMaxHealth = 100.0f;			//The health with no modifiers.
	public float maxHealthBoost = 0.0f;				//The max Health boosted by itens or powers.

	public float currentHealth;						//The player current health.

	#region PROPERTIES
	public float MaxHealth{
		get{
			return baseMaxHealth + maxHealthBoost;
		}
	}

	public float CurrentHealth {
		get{
			if (currentHealth > MaxHealth)
				currentHealth = MaxHealth;

			return currentHealth;
		}
	}
	#endregion
}
