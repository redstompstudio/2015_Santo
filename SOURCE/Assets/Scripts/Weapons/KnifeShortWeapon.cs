using UnityEngine;
using System.Collections;

public class KnifeShortWeapon : BaseWeapon
{
	public Transform hand_Left;
	public Transform hand_Right;

	private const string leftHand = "Hand_L";
	private const string rightHand = "Hand_R";

	public override void OnReceivedAttackEvent (string pMessage)
	{
		base.OnReceivedAttackEvent (pMessage);

		Vector3 searchPoint = hand_Left.position;
		Debug.DrawRay (searchPoint, Vector3.up * 50.0f, Color.red, 3.0f);

		switch(pMessage)
		{
		case leftHand:
			searchPoint = hand_Left.position;
			break;

		case rightHand:
			searchPoint = hand_Right.position;
			break;
		}

		Collider[] colliders = Physics.OverlapSphere (searchPoint, 0.25f, attackLayers);

		if(colliders != null)
		{
			for(int i = 0; i < colliders.Length; i++)
			{
				BaseActor actor = colliders [i].gameObject.GetComponent<BaseActor> ();

				if(actor)
				{
					if (weaponOwner != null)
						weaponOwner.ApplyDamage (actor, Damage, damageType, weaponPoint.position);
					else
						actor.ReceiveDamage (null, Damage, damageType, weaponPoint.position);
				}
			}
		}
	}
}
