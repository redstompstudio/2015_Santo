using UnityEngine;
using Prime31.StateKit;

public class DeadState : SKMecanimState<PlayerCharacterController>  
{
	private SpawnPool santoRagdollPool;
	private const string santoRagdollName = "Santo_Ragdoll_Pool";

	public override void begin ()
	{
		base.begin ();

		if (santoRagdollPool == null)
			santoRagdollPool = PoolManager.Instance.GetPool (santoRagdollName);

		_machine.animator.applyRootMotion = false;
		context.CharacterMotor.IsKinematic = true;
		context.CharacterMotor.UseGravity = true;

		santoRagdollPool.Spawn<ParticlePoolObject> (context.Position, context.Rotation);
		context.Kill ();
	}

	public override void update (float deltaTime, AnimatorStateInfo stateInfo)
	{
	}
}
