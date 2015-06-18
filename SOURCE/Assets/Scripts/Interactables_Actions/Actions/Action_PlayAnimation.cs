using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Action_PlayAnimation : BaseAction 
{
	private Animator animator;

	public string stateName;
	public float transionDuration = 0.1f;

	protected override void Awake ()
	{
		base.Awake ();

		if (animator == null)
			animator = GetComponent<Animator> ();
	}

	public override void StartAction ()
	{
		animator.CrossFade (stateName, transionDuration);
	}
}
