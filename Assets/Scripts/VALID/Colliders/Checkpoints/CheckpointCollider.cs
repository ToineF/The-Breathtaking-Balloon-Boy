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
            if (!_lastOtherCollider.transform.parent.parent.TryGetComponent(out CharacterManager character)) return;

            character.CheckpointManager.ChangeCurrentCheckPoint(_checkpoint);
            //character.CheckpointManager.SetToCheckpointPosition(character.CharacterRigidbody.gameObject);

        }
    }
}