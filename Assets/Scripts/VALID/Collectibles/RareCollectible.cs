using BlownAway.Character;
using BlownAway.Cutscenes;
using UnityEngine;

namespace BlownAway.Collectibles
{
    public class RareCollectible : Collectible
    {
        [Header("Rare Collectibles")]
        [SerializeField] private Cutscene _cutscene;

        protected override void OnDeath()
        {
            CharacterManager manager = _lastOtherCollider.GetComponent<CharacterCollider>()?.Manager;
            if (manager != null)
            {
                manager.Collectibles.AddRareCollectible();
                if (_cutscene != null) manager.CutsceneManager.StartNewSequence(_cutscene, manager);
                
                // Feedback
                manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.RareCollectibleFeedback);
            }
            base.OnDeath();
        }
    }
}