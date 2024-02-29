using UnityEngine;

namespace BlownAway.Hitbox.Checkpoints
{
    public class CheckpointManager : MonoBehaviour
    {
        [SerializeField] private Checkpoint _startCheckpoint;
        private Checkpoint _currentCheckPoint;

        private void Start()
        {
            if (_startCheckpoint != null)
                _currentCheckPoint = _startCheckpoint;
        }

        public void ChangeCurrentCheckPoint(Checkpoint checkPoint)
        {
            _currentCheckPoint = checkPoint;
        }

        public void SetToCheckpointPosition(GameObject objectToMove)
        {
            if (_currentCheckPoint == null) return;
            objectToMove.transform.position = _currentCheckPoint.transform.position;
        }
    }
}