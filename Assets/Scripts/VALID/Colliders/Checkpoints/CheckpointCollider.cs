using BlownAway.Character;
using UnityEngine;

namespace BlownAway.Hitbox.Checkpoints
{
    public class CheckpointCollider : BoxTrigger
    {
        [Header("Checkpoint")]
        [SerializeField] private Checkpoint _checkpoint;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += SetCurrentCheckpoint;
        }

        void SetCurrentCheckpoint()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            CharacterManager character = collider.Manager;

            character.CheckpointManager.ChangeCurrentCheckPoint(_checkpoint);
        }
    }
}