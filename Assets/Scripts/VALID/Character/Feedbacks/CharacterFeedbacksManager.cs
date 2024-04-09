using UnityEngine;

namespace BlownAway.Character.Feedbacks
{
    public class CharacterFeedbacksManager : CharacterSubComponent
    {
        [field:Header("References")]
        [field:SerializeField] public HapticManager HapticManager { get; private set; }

        [field:Header("Walk")]
        [field:SerializeField] public ParticleSystem WalkVFX { get; private set; }
    }

    public struct Feedback
    {
        public HapticFeedback HapticFeedback;
        //public ScreenShake HapticFeedback;
        public GameObject VFX;
        public AudioClip SFX;
    }
}