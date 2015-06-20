using UnityEngine;
using System.Collections;

public class SkullBulletFireController : SkullBulletController
{
	public float explosionRadius = 1.5f;
	public LayerMask explosionEffectLayers;

	protected override void OnTriggerEnter (Collider pOther)
	{
		if(tagsList.Contains(pOther.tag))
			Despawn ();
	}

	public override void Despawn ()
	{
		Collider[] colliders = Physics.OverlapSphere (Position, explosionRadius, explosionEffectLayers);

		foreach(Collider col in colliders)
		{
			BaseActor actor = col.GetComponent<BaseActor> ();

			if (actor != null)
				actor.ReceiveDamage (this, damage, damageType, Position);
		}

		base.Despawn ();
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (Position, explosionRadius);
	}
#endif
}
