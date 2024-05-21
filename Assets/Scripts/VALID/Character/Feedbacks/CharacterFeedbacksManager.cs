using UnityEngine;
using System;
using BlownAway.Camera;
using AntoineFoucault.Utilities;

namespace BlownAway.Character.Feedbacks
{
    public class CharacterFeedbacksManager : CharacterSubComponent
    {
        [field:Header("References")]
        [field:SerializeField] public HapticManager HapticManager { get; private set; }
        [field:SerializeField] public FMODAudioManager AudioManager { get; private set; }
        [field:SerializeField] public ScreenShake ScreenShake { get; private set; }

        [field:Header("VFX References")]
        [field:SerializeField] public ParticleSystem WalkVFX { get; private set; }
        [field:SerializeField] public ParticleSystem PropulsionVFX { get; private set; }
        [field:SerializeField] public ParticleSystem DeriveVFX { get; private set; }

        public void PlayFeedback(Feedback feedback)
        {
            PlayFeedback(feedback, Vector3.zero, Quaternion.identity, transform);
        }

        public void PlayFeedback(Feedback feedback, Vector3 position, Quaternion rotation, Transform transform)
        {
            // VFX
            if (feedback.VFX != null) Instantiate(feedback.VFX, position, rotation, transform);


            // SFX
            if (feedback.SFX.Length > 0) AudioManager?.PlayClip(feedback.SFX.GetRandomItem());

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
        public FMODUnity.EventReference[] SFX;
    }

    [Serializable]
    public struct ContinousFeedback
    {
        public Feedback Feedback;
        public float Frequency;
    }
}