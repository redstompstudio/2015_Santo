using UnityEngine;
using System.Collections.Generic;

public class SpikesScript : MonoBehaviour 
{
	private List<BaseActor> actorsInside = new List<BaseActor>();

	public int dps;
	public TriggerHandler trigger;

	private float lastDamageTime;

	private void Awake()
	{
		trigger.onTriggerEnterCallback += OnEnter;
		trigger.onTriggerExitCallback += OnExit;
	}

	private void Update()
	{
		if (lastDamageTime >= 1.0f) 
		{
			bool appliedDamage = false;

			foreach (BaseActor actor in actorsInside) 
			{
				actor.ReceiveDamage (null, dps, DAMAGE_TYPE.PIERCE, actor.Position);

				if (actor.Health.CurrentHealth <= 0.0f)
					actorsInside.Remove (actor);
				
				appliedDamage = true;
			}

			if (appliedDamage)
				DarkTonic.MasterAudio.MasterAudio.PlaySound ("Spikes");

			lastDamageTime = 0.0f;
		}

		lastDamageTime += Time.deltaTime;
	}

	void OnEnter(Collider pOther)
	{
		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null && !actorsInside.Contains(actor))
		{
			actorsInside.Add (actor);
			lastDamageTime = 1.0f;
		}
	}

	void OnExit(Collider pOther)
	{
		BaseActor actor = pOther.GetComponent<BaseActor> ();

		if(actor != null && actorsInside.Contains(actor))
		{
			actorsInside.Remove (actor);
		}
	}
}
