using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public abstract class Collectible : SphereTrigger
    {
        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += PickUp;
        }

        private void PickUp()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            OnPickUp();

            Destroy(gameObject);
        }

        protected abstract void OnPickUp();
    }
}