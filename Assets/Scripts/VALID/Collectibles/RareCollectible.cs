using BlownAway.Character;
using BlownAway.Cutscenes;
using UnityEngine;
using DG.Tweening;

namespace BlownAway.Collectibles
{
    public class RareCollectible : Collectible
    {
        [Header("Rare Collectibles")]
        [SerializeField] private Cutscene _cutscene;
        [SerializeField] private float _scaleTime;


        protected override void OnPickUp()
        {
            transform.DOScale(Vector3.zero, _scaleTime).OnComplete(OnDeath);
        }

        protected override void OnDeath()
        {
            CharacterManager manager = _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager;
            if (manager != null)
            {
                manager.Collectibles.AddRareCollectible();
                if (_cutscene != null) manager.CutsceneManager.StartNewSequence(_cutscene, manager);
                
                // Feedback
                manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.RareCollectibleFeedback, transform.position, Quaternion.identity, null);
            }
            base.OnDeath();
        }
    }
}