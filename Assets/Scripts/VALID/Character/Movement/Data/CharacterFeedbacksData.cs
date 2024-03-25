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
        [field: SerializeField] public GameObject DashVFX { get; private set; }
    }
}
