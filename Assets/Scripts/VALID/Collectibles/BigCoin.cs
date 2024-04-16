using BlownAway.Character;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public class BigCoin : Collectible
    {
        public int CoinsNumber => _coinsNumber;

        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private float _coinDistance;
        [SerializeField] private int _coinsNumber;
        [SerializeField] private float _turnSpeed;

        protected override void OnPickUp()
        {
            SpawnCoins();
            OnDeath();
        }

        private void Update()
        {
            transform.eulerAngles += Vector3.up * _turnSpeed;
        }

        private void SpawnCoins()
        {
            float angleMagnitude = 360f / _coinsNumber;
            for (int i = 0; i < _coinsNumber; i++)
            {
                float angle = angleMagnitude * i;
                Vector2 targetOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                targetOffset = targetOffset.normalized * _coinDistance;
                Vector3 targetPosition = transform.position + new Vector3(targetOffset.x, 0, targetOffset.y);
                Instantiate(_coinPrefab, targetPosition, _coinPrefab.transform.rotation);
            }

            CharacterManager manager = _lastOtherCollider.GetComponent<CharacterManager>();
            if (manager != null) manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.BigCoinFeedback);
        }
    }
}