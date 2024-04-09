using Cinemachine;
using System.Collections;
using UnityEngine;

namespace BlownAway.Camera
{
    public class ScreenShake : MonoBehaviour
    {
        private Cinemachine.CinemachineVirtualCamera _cinemachineVirtualCamera;
        [Tooltip("The strength of the screenshake")][SerializeField] private float _strength = 1f;
        [Tooltip("The length of the screenshake in seconds")][SerializeField] private float _duration = 1f;
        [Tooltip("The curve of the screenshake's strength over time")][SerializeField] private AnimationCurve _curve;

        private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            _cinemachinePerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            _cinemachinePerlin.m_AmplitudeGain = 0;
        }

        public void Shake(float strength = -1, float duration = -1, AnimationCurve curve = null)
        {
            StartCoroutine(Shaking(strength, duration, curve));
        }

        private IEnumerator Shaking(float strength = -1, float duration = -1, AnimationCurve curve = null)
        {
            if (strength == -1) strength = this._strength;
            if (duration == -1) duration = this._duration;
            if (curve == null) curve = this._curve;

            _cinemachinePerlin.m_AmplitudeGain = _strength;

            yield return new WaitForSeconds(_duration);

            _cinemachinePerlin.m_AmplitudeGain = 0;
        }
    }
}