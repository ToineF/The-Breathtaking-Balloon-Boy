using UnityEngine;
using System;
using BlownAway.Player;
using BlownAway.Hitbox;

namespace BlownAway.GPE
{
    [RequireComponent(typeof(Collider))]
    public class OLDWindZone : HitboxTrigger
    {
        [SerializeField] [Tooltip("The Magnitude at which the Wind pushes the Player")] private float _pushMagnitude;
        [SerializeField] [Tooltip("The normalized direction of the Wind")] private Vector3 _pushVector;
        [SerializeField] [Tooltip("Is the Wind hot or cold?")] private bool _isHot;
        [SerializeField] [Tooltip("The percentage of air the player gains or loses with every contact (postive only)")] private float _airPercentageAddedOnContact;
        [SerializeField] [Tooltip("The interval of time in seconds at which the wind collides with the player")] private float _timeBetweenAdditions;
        [SerializeField] [Tooltip("The acceleration of the force towards the magnitude given to the player")] [Range(0, 1)] private float _startLerpValue;
        [SerializeField] [Tooltip("The deceleration of the force from the magnitude given to the player")] [Range(0, 1)] private float _stopLerpValue;
        [SerializeField] [Tooltip("The original scale used for reference to scale the VFX")] private Vector3 _targetScale;
        private float _timer;

        [Header("FX")]
        [SerializeField] [Tooltip("A reference to the Particule Effect of the Wind")] private GameObject _FXWind;
        [SerializeField] [Tooltip("A reference to all of the Particules of the Effect")] private ParticleSystem[] _FXWinds;
        [SerializeField] [Tooltip("The Color of the Hot Air")] private Color _HotColor;
        [SerializeField] [Tooltip("The Color of the Cold Air")] private Color _ColdColor;

        [Header("Sound")]
        [SerializeField] private AudioSource _windBlowingSound;
        [SerializeField] private float _audioStartFadeTime;
        [SerializeField] private float _audioEndFadeTime;

        private Vector3 _lastForce;
        private bool _isPlayerIn;

        private void Start()
        {
            _timer = _timeBetweenAdditions;
            _pushVector = new Vector3(_pushVector.x, _isHot ? 1 : -1 * Math.Abs(_pushVector.y), _pushVector.z);

            var fxWindZ = _pushVector.x > 0 ? -90 : _pushVector.x < 0 ? 90 : 0;
            var fxWindX = _pushVector.z < 0 ? -90 : _pushVector.z > 0 ? 90 : 0;
            fxWindZ = _pushVector.y < 0 ? 180 : fxWindZ;
            _FXWind.transform.eulerAngles = new Vector3(fxWindX, 0, fxWindZ);
            _FXWind.transform.position += _pushVector.y < 0 ? Vector3.up * GetComponent<Collider>().bounds.size.y : Vector3.zero;

            foreach (var fx in _FXWinds)
            {
                Color color = _isHot ? _HotColor : _ColdColor;
                fx.startColor = color;
                fx.transform.localScale = new Vector3(transform.localScale.x / _targetScale.x, transform.localScale.y / _targetScale.y, transform.localScale.z / _targetScale.z);
            }

        }
        override protected void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest character)) return;
           /* BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
            SetHammerBalloonForce(balloonStateManager, _pushVector.y);
            if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;*/

            if (!_isPlayerIn) _lastForce = character.CurrentForce;
            //character.SetForce(_pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
            character.AddAdditionalForce(gameObject, _pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
            _isPlayerIn = true;

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.FadeAudioSourceVolume(_windBlowingSound, _audioStartFadeTime, 1);

        }

        override protected void OnTriggerStay(Collider other)
        {
            if (_timer > 0) return;

            if (!other.TryGetComponent(out CharacterControllerTest character)) return;
            /*BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
            SetHammerBalloonForce(balloonStateManager, _pushVector.y);
            if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;*/

            if (!_isPlayerIn) _lastForce = character.CurrentForce;
            //character.SetForce(_pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
            character.AddAdditionalForce(gameObject, _pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
            _isPlayerIn = true;

            /*int airSign = _isHot ? 1 : -1;
            AirManager.Instance.AddAir(_airPercentageAddedOnContact * airSign);*/
            _timer = _timeBetweenAdditions;
        }

        //private void SetHammerBalloonForce(BalloonStateManager balloonStateManager, float force)
        //{
        //    if (balloonStateManager.GetState() == balloonStateManager.BalloonHammer)
        //    {
        //        balloonStateManager.HammerWindForce = force;
        //    }
        //}

        override protected void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out CharacterControllerTest character)) return;
            /*BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
            SetHammerBalloonForce(balloonStateManager, 0);
            if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;*/


            //character.SetForce(_lastForce, _stopLerpValue);
            character.AddAdditionalForce(gameObject, Vector3.zero ,_stopLerpValue);
            _isPlayerIn = false;

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.FadeAudioSourceVolume(_windBlowingSound, _audioEndFadeTime, 0);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }
    }
}
