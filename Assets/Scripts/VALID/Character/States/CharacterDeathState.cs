using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("DEATH");
            manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterRigidbody.gameObject);

            manager.States.SwitchState(manager.States.IdleState);
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
