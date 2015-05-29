using UnityEngine;
using System.Collections;

public class RifleAim_Behaviour : BaseStateMachineBehaviour 
{
	public override void OnStateIK (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateIK (animator, stateInfo, layerIndex);

		if (onStateIKCallback != null)
			onStateIKCallback ();
	}
}
