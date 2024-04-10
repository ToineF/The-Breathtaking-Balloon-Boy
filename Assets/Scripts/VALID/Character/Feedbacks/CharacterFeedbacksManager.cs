using UnityEngine;
using System;
using BlownAway.Camera;

namespace BlownAway.Character.Feedbacks
{
    public class CharacterFeedbacksManager : CharacterSubComponent
    {
        [field:Header("References")]
        [field:SerializeField] public HapticManager HapticManager { get; private set; }
        [field:SerializeField] public FMODAudioManager AudioManager { get; private set; }
        [field:SerializeField] public ScreenShake ScreenShake { get; private set; }

        [field:Header("Walk")]
        [field:SerializeField] public ParticleSystem WalkVFX { get; private set; }

        public void PlayFeedback(Feedback feedback)
        {
            // VFX
            if (feedback.VFX != null) Instantiate(feedback.VFX);

            // SFX
            AudioManager?.PlayClip(feedback.SFX);

            // Haptic
            HapticManager?.VibrateForTime(feedback.HapticFeedback);

            // Screenshake
            ScreenShake?.Shake(feedback.ShakeFeedback);
        }

        public void PlayFeedback(Feedback feedback, Vector3 position, Quaternion rotation, Transform transform)
        {
            // VFX
            if (feedback.VFX != null) Instantiate(feedback.VFX, position, rotation, transform);


            // SFX
            AudioManager?.PlayClip(feedback.SFX);

            // Haptic
            HapticManager?.VibrateForTime(feedback.HapticFeedback);

            // Screenshake
            ScreenShake?.Shake(feedback.ShakeFeedback);
        }
    }

    [Serializable]
    public struct Feedback
    {
        public HapticFeedback HapticFeedback;
        public ShakeData ShakeFeedback;
        public GameObject VFX;
        public FMODUnity.EventReference SFX;
    }
}