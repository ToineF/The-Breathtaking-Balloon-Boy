using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace BlownAway.Collectibles
{
    public abstract class Collectible : SphereTrigger
    {
        protected CharacterCollider _owner;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += PickUp;
        }

        private void PickUp()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            _owner = collider;

            OnPickUp();
        }

        protected virtual void OnPickUp()
        {
            Destroy(gameObject);
        }
    }
}