using BlownAway.Character;
using System.Collections;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public class Coin : MagneticCollectible
    {
        [Header("Coin")]
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _uiWinDelay;
        [SerializeField] private bool _muteAudioFeedback;

        protected override void OnPickUp()
        {
            CharacterCollider collider = _lastOtherCollider.GetComponent<CharacterCollider>();
            if (collider != null)
            {
                CharacterManager manager = collider.Manager;
                manager.Collectibles.AddCoinPreview();
                manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.CoinPreviewFeedback, transform.position, Quaternion.identity, null, _muteAudioFeedback);

                StartCoroutine(AddCoinToCount(manager));
            }
        }

        private void Update()
        {
            transform.eulerAngles += Vector3.up * _turnSpeed * Time.deltaTime;
            LerpTowardsPlayer();
        }


        private IEnumerator AddCoinToCount(CharacterManager manager)
        {
            yield return new WaitForSeconds(_uiWinDelay);
            _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager.Collectibles.AddCoin();
            manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.CoinFeedback, transform.position, Quaternion.identity, null, _muteAudioFeedback);

        }
    }
}