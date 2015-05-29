using UnityEngine;
using System.Collections;

public class BowArrowWeapon : BaseWeapon
{
	public Transform arrowPosition;

	public override void Attack ()
	{
		base.Attack ();

		var arrow = TrashMan.spawn ("Arrow", arrowPosition.position, arrowPosition.rotation);

		TrashMan.recycleBinForGameObjectName(arrow.name).onDespawnedEvent += OnDespawnArrow;
		TrashMan.despawnAfterDelay( arrow, 3.0f );

		Rigidbody arrowPhysics = arrow.GetComponent<Rigidbody> ();

		if(arrowPhysics != null)
		{
			arrowPhysics.AddForce (arrow.transform.forward * 900.0f);
		}
	}

	void OnDespawnArrow(GameObject pGo)
	{
		pGo.GetComponent<Rigidbody> ().Sleep ();
	}
}
