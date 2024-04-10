using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

namespace BlownAway.Camera
{
    public class ScreenShake : MonoBehaviour
    {
        private Cinemachine.CinemachineVirtualCamera _cinemachineVirtualCamera;
        [Tooltip("The strength of the screenshake")][SerializeField] private float _strength = 1f;
        [Tooltip("The length of the screenshake in seconds")][SerializeField] private float _duration = 1f;

        private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;
        private Coroutine _shakeCoroutine;

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            _cinemachinePerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            _cinemachinePerlin.m_AmplitudeGain = 0;
        }

        public void Shake(float strength = -1, float duration = -1)
        {
            if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = StartCoroutine(Shaking(strength, duration));
        }

        public void Shake(ShakeData data)
        {
            Shake(data.Strength, data.Duration);
        }

        private IEnumerator Shaking(float strength = -1, float duration = -1)
        {
            if (strength == -1) strength = _strength;
            if (duration == -1) duration = _duration;

            _cinemachinePerlin.m_AmplitudeGain = strength;

            yield return new WaitForSeconds(duration);

            _cinemachinePerlin.m_AmplitudeGain = 0;
        }
    }

    [Serializable]
    public struct ShakeData
    {
        public float Strength;
        public float Duration;
    }
}