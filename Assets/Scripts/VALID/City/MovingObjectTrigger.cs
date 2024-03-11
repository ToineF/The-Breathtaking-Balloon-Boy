using UnityEngine;
using System;
using AntoineFoucault.Utilities;


namespace BlownAway.City
{
    public class MovingObjectTrigger : BoxTrigger
    {
        [Header("Moving Object Params")]
        [SerializeField] private MovingObject _object;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += MakePlayerBounce;
        }

        private void MakePlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _object.StartMoving();

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
    }
}