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

        [field: Header("Fall")]
        [field: SerializeField] public GameObject LandVFX { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField] public Feedback DashFeedback { get; private set; }

        [field: Header("Balloon Bounce")]
        [field: SerializeField] public Feedback BalloonBounceFeedback { get; private set; }

        [field: Header("Collectibles")]
        [field: SerializeField] public Feedback CoinFeedback { get; private set; }
        [field: SerializeField] public Feedback BigCoinFeedback { get; private set; }
        [field: SerializeField] public Feedback RareCollectibleFeedback { get; private set; }
    }
}
