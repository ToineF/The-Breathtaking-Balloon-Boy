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

        private IEnumerator VibrateController(float time)
        {
            yield return new WaitForSeconds(time);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}