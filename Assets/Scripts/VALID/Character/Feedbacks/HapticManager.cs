using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlownAway.Character.Feedbacks
{
    public class HapticManager : MonoBehaviour
    {
        /// <summary>
        /// Start vibrating a gamepad for a set time in seconds
        /// </summary>
        /// <param name="lowFrequency"></param>
        /// <param name="highFrequency"></param>
        /// <param name="time"></param>
        public void VibrateForTime(float lowFrequency, float highFrequency, float time)
        {
            if (Gamepad.current == null) return;
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(VibrateController(time));
        }

        /// <summary>
        /// Start vibrating a gamepad for a set time in seconds
        /// </summary>
        /// <param name="feedback"></param>
        public void VibrateForTime(HapticFeedback feedback)
        {
            if (Gamepad.current == null) return;
            Gamepad.current.SetMotorSpeeds(feedback.LowFrequency, feedback.HighFrequency);
            StartCoroutine(VibrateController(feedback.Time));
        }

        private IEnumerator VibrateController(float time)
        {
            yield return new WaitForSeconds(time);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }

    [Serializable]
    public struct HapticFeedback
    {
        public float LowFrequency;
        public float HighFrequency;
        public float Time;

        public HapticFeedback(float low, float high, float time)
        {
            LowFrequency = low;
            HighFrequency = high;
            Time = time;
        }
    }
}