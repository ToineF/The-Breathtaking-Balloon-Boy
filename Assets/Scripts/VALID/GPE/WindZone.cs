using UnityEngine;
using System;
using AntoineFoucault.Utilities;

namespace BlownAway.GPE
{
    public class WindZone : BoxTrigger
    {
        [Header("WindZone Params")]
        [SerializeField][Tooltip("The Magnitude at which the Wind pushes the Player")] private float _pushMagnitude;
        [SerializeField][Tooltip("The normalized direction of the Wind")] private Vector3 _pushVector;
        //[SerializeField][Tooltip("Is the Wind hot or cold?")] private bool _isHot;
        //[SerializeField][Tooltip("The percentage of air the player gains or loses with every contact (postive only)")] private float _airPercentageAddedOnContact;
        //[SerializeField][Tooltip("The interval of time in seconds at which the wind collides with the player")] private float _timeBetweenAdditions;
        [SerializeField][Tooltip("The acceleration of the force towards the magnitude given to the player")][Range(0, 1)] private float _startLerpValue;
        [SerializeField][Tooltip("The deceleration of the force from the magnitude given to the player")][Range(0, 1)] private float _stopLerpValue;
        //[SerializeField][Tooltip("The original scale used for reference to scale the VFX")] private Vector3 _targetScale;

        //[Header("FX")]
        //[SerializeField][Tooltip("A reference to the Particule Effect of the Wind")] private GameObject _FXWind;
        //[SerializeField][Tooltip("A reference to all of the Particules of the Effect")] private ParticleSystem[] _FXWinds;
        //[SerializeField][Tooltip("The Color of the Hot Air")] private Color _HotColor;
        //[SerializeField][Tooltip("The Color of the Cold Air")] private Color _ColdColor;

        //[Header("Sound")]
        //[SerializeField] private AudioSource _windBlowingSound;
        //[SerializeField] private float _audioStartFadeTime;
        //[SerializeField] private float _audioEndFadeTime;

        private float _timer;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += MakePlayerBounce;
            OnExitTrigger += StopPlayerBounce;
        }

        private void MakePlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            Vector3 force = _pushVector.normalized * _pushMagnitude;

            collider.Manager.MovementManager.AddExternalForce(gameObject, force, _startLerpValue);

        }

        private void StopPlayerBounce()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            collider.Manager.MovementManager.AddExternalForce(gameObject, Vector3.zero, _stopLerpValue);

        }

        private new void OnDrawGizmos()
        {
            if (!_displayGizmos) return;
            if (_showOnlyWhileSelected) return;

            base.OnDrawGizmos();

            Vector3 position = transform.position;

            GizmoExtensions.DrawArrow(position, _pushVector.normalized);
        }

        private new void OnDrawGizmosSelected()
        {
            if (!_displayGizmos) return;

            base.OnDrawGizmosSelected();

            Vector3 position = transform.position;

            GizmoExtensions.DrawArrow(position, _pushVector.normalized);
        }

        //private bool _isPlayerIn;

        //private void Start()
        //{
        //    //_timer = _timeBetweenAdditions;
        //    _pushVector = new Vector3(_pushVector.x, _isHot ? 1 : -1 * Math.Abs(_pushVector.y), _pushVector.z);
        //}

        //override protected void OnTriggerEnter(Collider other)
        //{
        //    if (!other.TryGetComponent(out CharacterControllerTest character)) return;

        //    character.AddAdditionalForce(gameObject, _pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
        //    _isPlayerIn = true;

        //    // Sound
        //    if (AudioManager.Instance != null)
        //        AudioManager.Instance.FadeAudioSourceVolume(_windBlowingSound, _audioStartFadeTime, 1);

        //}

        ////override protected void OnTriggerStay(Collider other)
        ////{
        ////    if (_timer > 0) return;

        ////    if (!other.TryGetComponent(out CharacterControllerTest character)) return;

        ////    character.AddAdditionalForce(gameObject, _pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
        ////    _isPlayerIn = true;

        ////    _timer = _timeBetweenAdditions;
        ////}

        //override protected void OnTriggerExit(Collider other)
        //{
        //    if (!other.TryGetComponent(out CharacterControllerTest character)) return;

        //    character.AddAdditionalForce(gameObject, Vector3.zero, _stopLerpValue);
        //    _isPlayerIn = false;

        //    // Sound
        //    if (AudioManager.Instance != null)
        //        AudioManager.Instance.FadeAudioSourceVolume(_windBlowingSound, _audioEndFadeTime, 0);
        //}

        ////private void Update()
        ////{
        ////    _timer -= Time.deltaTime;
        ////}
    }
}
