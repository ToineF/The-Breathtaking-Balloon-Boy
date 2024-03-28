using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

namespace BlownAway.Collectibles
{
    public class Coin : Collectible
    {
        [Header("Coin")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _magnetSpeed;
        [SerializeField] private AnimationCurve _magnetCurve;

        private float _magnetTimer = 0;

        protected override void OnPickUp()
        {
            
        }

        private void Update()
        {
            transform.eulerAngles += Vector3.up * _turnSpeed;
            LerpTowardsPlayer();
        }

        private void LerpTowardsPlayer()
        {
            if (_owner == null) return;

            _magnetTimer += Time.deltaTime;
            float percentile = _magnetTimer / _magnetSpeed;
            float weight = _magnetCurve.Evaluate(percentile);
            transform.position = Vector3.Lerp(transform.position, _owner.transform.position, weight);

            if (percentile > 1) Destroy(gameObject);
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}