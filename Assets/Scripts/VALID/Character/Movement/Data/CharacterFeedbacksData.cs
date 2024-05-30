using BlownAway.Character.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "FeedbackData", menuName = "CharacterData/FeedbackData")]
    public class CharacterFeedbacksData : ScriptableObject
    {
        [field: Header("Walk")]
        [field: SerializeField] public GameObject WalkStartVFX { get; private set; }
        [field: SerializeField] public ContinousFeedback WalkContinousFeedback { get; private set; }

        [field: Header("Fall")]
        [field: SerializeField] public Feedback LandingFeedback { get; private set; }

        [field:Header("Jump")]
        [field: SerializeField] public Feedback JumpFeedback { get; private set; }

        [field: Header("Propulsion")]
        [field: SerializeField] public Feedback StartPropulsionFeedback { get; private set; }
        [field: SerializeField] public Feedback CancelFloatFeedback { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField] public Feedback DashFeedback { get; private set; }

        [field: Header("Balloon Bounce")]
        [field: SerializeField] public Feedback BalloonBounceFeedback { get; private set; }

        [field:Header("Death")]
        [field: SerializeField] public Feedback DeathFeedback { get; private set; }

        [field: Header("Collectibles")]
        [field: SerializeField] public Feedback CoinPreviewFeedback { get; private set; }
        [field: SerializeField] public Feedback CoinFeedback { get; private set; }
        [field: SerializeField] public Feedback BigCoinFeedback { get; private set; }
        [field: SerializeField] public Feedback RareCollectibleFeedback { get; private set; }
    }
}
