using UnityEngine;

namespace BlownAway.Collectibles
{
    public class Coin : Collectible
    {
        [Header("Coin")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _magnetSpeed;
        [SerializeField] private AnimationCurve _magnetCurve;
        [SerializeField] private AnimationCurve _scaleOverTime;

        private float _magnetTimer = 0;

        protected override void OnPickUp()
        {
            _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager.Collectibles.AddCoin();
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
            float positionWeight = _magnetCurve.Evaluate(percentile);
            float scaleWeight = _scaleOverTime.Evaluate(percentile);
            transform.position = Vector3.Lerp(transform.position, _owner.Collider.bounds.center, positionWeight);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, scaleWeight);

            if (percentile > 1) OnDeath();
        }

        protected override void OnDeath()
        {
            base.OnDeath();
        }
    }
}