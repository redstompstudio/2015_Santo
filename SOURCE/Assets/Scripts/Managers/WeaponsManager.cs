using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponsManager : MonoBehaviour 
{
	private static WeaponsManager instance;

	public List<BaseWeapon> weapons = new List<BaseWeapon>();

	public static WeaponsManager Instance{
		get{
			if (instance == null)
				instance = FindObjectOfType (typeof(WeaponsManager)) as WeaponsManager;
			return instance;
		}
	}

	void Awake()
	{
		if (instance == null)
			instance = this;
	}
}
