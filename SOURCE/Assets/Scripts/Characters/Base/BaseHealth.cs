using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseHealth 
{
	public int baseMaxHealth = 100;			//The health with no modifiers.
	public int maxHealthBoost = 0;				//The max Health boosted by itens or powers.

	public int currentHealth = 100;						//The player current health.

	public delegate void OnDeathDelegate ();
	public OnDeathDelegate onDeathCallback;

	#region PROPERTIES
	public int MaxHealth{
		get{
			return baseMaxHealth + maxHealthBoost;
		}
	}

	public int CurrentHealth {
		get{
			if (currentHealth > MaxHealth)
				currentHealth = MaxHealth;

			return currentHealth;
		}
	}
	#endregion

	public void DoDamage(int pDamage)
	{
		currentHealth = CurrentHealth - pDamage;

		if (currentHealth <= 0.0f) 
		{
			if(onDeathCallback != null)
				onDeathCallback ();
		}
	}

	public void Heal(int pAmount)
	{
		currentHealth = CurrentHealth + pAmount;
	}
}
