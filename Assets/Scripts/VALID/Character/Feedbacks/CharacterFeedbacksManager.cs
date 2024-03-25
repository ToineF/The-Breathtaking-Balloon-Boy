using UnityEngine;

namespace BlownAway.Character.Movements
{
    public class CharacterFeedbacksManager : CharacterSubComponent
    {
        [field:Header("Walk")]
        [field:SerializeField] public ParticleSystem WalkVFX { get; private set; }
    }
}