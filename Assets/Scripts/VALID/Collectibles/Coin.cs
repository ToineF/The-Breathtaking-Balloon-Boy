using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlownAway.Collectibles
{
    public class Coin : Collectible
    {
        [Header("Coin")]
        [SerializeField] private float _turnSpeed;

        protected override void OnPickUp()
        {

        }

        private void Update()
        {
            transform.eulerAngles += Vector3.up * _turnSpeed;
        }
    }
}