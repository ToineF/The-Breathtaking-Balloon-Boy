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

        [Header("FX")]
        //[SerializeField][Tooltip("A reference to the Particule Effect of the Wind")] private GameObject _FXWind;
        //[SerializeField][Tooltip("A reference to all of the Particules of the Effect")] private ParticleSystem[] _FXWinds;
        //[SerializeField][Tooltip("The Color of the Hot Air")] private Color _HotColor;
        //[SerializeField][Tooltip("The Color of the Cold Air")] private Color _ColdColor;
        [SerializeField][Tooltip("A reference to all of the Particules of the Effect")] private ParticleSystem _FXWind;
        [SerializeField][Tooltip("The rotation offset to the FX Particules")] private float _FXWindRotationOffset = -90;
        [SerializeField][Tooltip("The emission of particules over time for a (1,1,1) cube")] private float _FXWindParticulesCount = 100;


        //[Header("Sound")]
        //[SerializeField] private AudioSource _windBlowingSound;
        //[SerializeField] private float _audioStartFadeTime;
        //[SerializeField] private float _audioEndFadeTime;

        private float _timer;

        private new void Awake()
        {
            base.Awake();
            OnEnterTrigger += StartPlayerPush;
            OnExitTrigger += StopPlayerPush;
        }

        private void Start()
        {
            _FXWind.transform.localScale = Vector3.Scale(transform.localScale, _FXWind.transform.localScale);
            //_FXWind.transform.rotation = Quaternion.FromToRotation(Vector3.up, _pushVector);

            //    ParticleSystem.ShapeModule editableShape = _FXWind.shape;
            //ParticleSystem.MainModule main = _FXWind.main;
            //    ParticleSystem.EmissionModule emission = _FXWind.emission;
            //    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = _FXWind.velocityOverLifetime;

            //    Vector3 scale = transform.localScale;

            //    _FXWind.transform.position = transform.position;
            //    _FXWind.transform.localScale = transform.localScale;
            //    main.startSizeX = new ParticleSystem.MinMaxCurve(main.startSizeX.constant * scale.x);
            //    main.startSizeY = new ParticleSystem.MinMaxCurve(main.startSizeY.constant * scale.y);
            //    main.startSizeZ = new ParticleSystem.MinMaxCurve(main.startSizeZ.constant * scale.z);
            //    //velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(main.startSize.constantMin * scale, main.startSize.constantMax * scale);

            //main.startRotationX = ((_pushVector.y * -90) + (_pushVector.z < 0 ? _pushVector.z * 180 : 0)) * Mathf.Deg2Rad;
            //main.startRotationY = (_pushVector.x * 90) * Mathf.Deg2Rad;
            _FXWind.Play();

            //    velocityOverLifetime.x = velocityOverLifetime.x.constant * _pushVector.x;
            //    velocityOverLifetime.y = velocityOverLifetime.y.constant * _pushVector.y;
            //    velocityOverLifetime.z = velocityOverLifetime.z.constant * _pushVector.z;
            //    emission.rateOverTime = _FXWindParticulesCount * scale.magnitude;
        }

        private void StartPlayerPush()
        {
            if (!_lastOtherCollider.TryGetComponent(out CharacterCollider collider)) return;

            Vector3 force = _pushVector.normalized * _pushMagnitude;

            collider.Manager.MovementManager.AddExternalForce(gameObject, force, _startLerpValue);

        }

        private void StopPlayerPush()
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
