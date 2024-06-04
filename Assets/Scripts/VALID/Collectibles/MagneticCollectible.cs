
using UnityEngine;

namespace BlownAway.Collectibles
{
    public abstract class MagneticCollectible : Collectible
    {
        [SerializeField] protected float _magnetSpeedMin;
        [SerializeField] protected float _magnetSpeedMax;
        [SerializeField] protected AnimationCurve _magnetCurve;
        [SerializeField] protected AnimationCurve _scaleOverTime;

        private float _magnetTimer = 0;
        private float _magnetSpeed;
        private Vector3 _startPosition;
        private Vector3 _startScale;

        private void Start()
        {
            _startPosition = transform.position;
            _startScale = transform.localScale;
            _magnetSpeed = Random.Range(_magnetSpeedMin, _magnetSpeedMax);
        }

        protected void LerpTowardsPlayer()
        {
            if (_owner == null) return;

            _magnetTimer += Time.deltaTime;
            float percentile = _magnetTimer / _magnetSpeed;
            float positionWeight = _magnetCurve.Evaluate(percentile);
            float scaleWeight = _scaleOverTime.Evaluate(percentile);
            transform.position = _startPosition + (_owner.Collider.bounds.center - _startPosition) * positionWeight;
            transform.localScale = _startScale * (1f - scaleWeight);

            if (percentile > 1) OnDeath();
        }
    }
}