using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public class RareCollectible : Collectible
    {
        protected override void OnDeath()
        {
            _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager.Collectibles.AddRareCollectible();
            base.OnDeath();
        }
    }
}