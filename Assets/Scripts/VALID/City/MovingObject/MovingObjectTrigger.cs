using UnityEngine;
using BlownAway.Character;

namespace BlownAway.City
{
    public class MovingObjectTrigger : BoxTrigger
    {
        [Header("Moving Object Params")]
        [SerializeField] private MovingObject _object;

        private CharacterManager _characterManager;


        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += CheckForPlayerCollisions;
        }


        private void CheckForPlayerCollisions()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _object.StartMoving();
            _characterManager = collider.Manager;
            _characterManager.MovementManager.OnDeath += OnDeath;
        }

        private new void OnDrawGizmos()
        {
            if (!_displayGizmos) return;
            if (_showOnlyWhileSelected) return;

            base.OnDrawGizmos();
        }

        private new void OnDrawGizmosSelected()
        {
            if (!_displayGizmos) return;

            base.OnDrawGizmosSelected();
        }

        private void OnDeath(CharacterManager manager)
        {
            _object.ResetPosition();
            if (_characterManager != null) _characterManager.MovementManager.OnDeath -= OnDeath;
        }
    }
}