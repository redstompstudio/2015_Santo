using UnityEngine;
using System.Collections;

public class BowArrowWeapon : BaseWeapon
{
	public Transform arrowPosition;

	public override void Attack ()
	{
		base.Attack ();

		var arrow = TrashMan.spawn ("Arrow", arrowPosition.position, Quaternion.identity);
		TrashMan.despawnAfterDelay( arrow, 3.0f );

		arrow.GetComponent<Rigidbody> ().AddForce(arrowPosition.forward * 200.0f);
	}
}
