using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "FeedbackData", menuName = "CharacterData/FeedbackData")]
    public class CharacterFeedbacksData : ScriptableObject
    {
        [field: Header("Dash")]
        [field: SerializeField] public GameObject DashVFX { get; private set; }
    }
}
