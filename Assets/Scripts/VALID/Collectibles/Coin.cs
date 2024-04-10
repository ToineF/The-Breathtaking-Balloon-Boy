using System.Collections;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public class Coin : Collectible
    {
        [Header("Coin")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _magnetSpeedMin;
        [SerializeField] private float _magnetSpeedMax;
        [SerializeField] private AnimationCurve _magnetCurve;
        [SerializeField] private AnimationCurve _scaleOverTime;
        [SerializeField] private float _uiWinDelay;

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

        protected override void OnPickUp()
        {
            _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager.Collectibles.AddCoinPreview();
            StartCoroutine(AddCoinToCount());
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
            transform.position = _startPosition + (_owner.Collider.bounds.center - _startPosition) * positionWeight;
            transform.localScale = _startScale * (1f-scaleWeight);

            if (percentile > 1) OnDeath();
        }

        private IEnumerator AddCoinToCount()
        {
            yield return new WaitForSeconds(_uiWinDelay);
            _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager.Collectibles.AddCoin();
        }
    }
}