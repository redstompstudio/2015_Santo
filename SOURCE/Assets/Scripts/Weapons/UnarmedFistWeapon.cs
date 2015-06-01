using UnityEngine;
using System.Collections;

public class UnarmedFistWeapon : BaseWeapon
{
	public Transform handPosition;

	public override void OnReceivedAttackEvent (string pMessage)
	{
		base.OnReceivedAttackEvent (pMessage);

		var particle = TrashMan.spawn ("Punch_HitText_FX", handPosition.position, Quaternion.identity);	
		TrashMan.despawnAfterDelay (particle, 2.0f);

		particle = TrashMan.spawn ("Punch_Hit_FX", handPosition.position, Quaternion.identity);	
		TrashMan.despawnAfterDelay (particle, 2.0f);
	}
}
