using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    [SerializeField] [Tooltip("The Magnitude at which the Wind pushes the Player")] private float _pushMagnitude;
    [SerializeField] [Tooltip("The normalized direction of the Wind")] private Vector3 _pushVector;
    [SerializeField] [Tooltip("Is the Wind hot or cold?")] private bool _isHot;
    [SerializeField] [Tooltip("The percentage of air the player gains or loses with every contact (postive only)")] private float _airPercentageAddedOnContact;
    [SerializeField] [Tooltip("The interval of time in seconds at which the wind collides with the player")] private float _timeBetweenAdditions;
    [SerializeField] [Tooltip("The acceleration of the force towards the magnitude given to the player")] [Range(0, 1)] private float _startLerpValue;
    [SerializeField] [Tooltip("The deceleration of the force from the magnitude given to the player")] [Range(0, 1)] private float _stopLerpValue;
    private float _timer;

    [Header("FX")]
    [SerializeField] [Tooltip("A reference to the Particule Effect of the Wind")] private GameObject _FXWind;
    [SerializeField] [Tooltip("A reference to all of the Particules of the Effect")] private ParticleSystem[] _FXWinds;
    [SerializeField] [Tooltip("The Color of the Hot Air")] private Color _HotColor;
    [SerializeField] [Tooltip("The Color of the Cold Air")] private Color _ColdColor;

    private Vector3 _lastForce;
    private bool _isPlayerIn;

    private void Start()
    {
        _timer = _timeBetweenAdditions;
        _pushVector = new Vector3(_pushVector.x, _isHot?1:-1 * Math.Abs(_pushVector.y), _pushVector.z);

        var fxWindZ = _pushVector.x > 0 ? -90 : _pushVector.x < 0 ? 90 : 0;
        var fxWindX = _pushVector.z < 0 ? -90 : _pushVector.z > 0 ? 90 : 0;
        fxWindZ = _pushVector.y < 0 ? 180 : fxWindZ;
        _FXWind.transform.eulerAngles = new Vector3(fxWindX, 0, fxWindZ);
        _FXWind.transform.position += _pushVector.y < 0 ? Vector3.up * GetComponent<Collider>().bounds.size.y : Vector3.zero;

        foreach (var fx in _FXWinds)
        {
            Color color = _isHot ? _HotColor : _ColdColor;
            fx.startColor = color;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterControllerTest>() == null) return;
        BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
        SetHammerBalloonForce(balloonStateManager, _pushVector.y);
        if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;

        if (!_isPlayerIn) _lastForce = other.GetComponent<CharacterControllerTest>().CurrentForce;
        other.GetComponent<CharacterControllerTest>().SetForce(_pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
        _isPlayerIn = true;

    }

    private void OnTriggerStay(Collider other)
    {
        if (_timer > 0) return;

        if (other.GetComponent<CharacterControllerTest>() == null) return;
        BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
        SetHammerBalloonForce(balloonStateManager, _pushVector.y);
        if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;

        if (!_isPlayerIn) _lastForce = other.GetComponent<CharacterControllerTest>().CurrentForce;
        other.GetComponent<CharacterControllerTest>().SetForce(_pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
        _isPlayerIn = true;

        int airSign = _isHot ? 1 : -1;
        AirManager.Instance.AddAir(_airPercentageAddedOnContact * airSign);
        _timer = _timeBetweenAdditions;
    }

    private void SetHammerBalloonForce(BalloonStateManager balloonStateManager, float force)
    {
        if (balloonStateManager.GetState() == balloonStateManager.BalloonHammer)
        {
            balloonStateManager.HammerWindForce = force;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterControllerTest>() == null) return;
        BalloonStateManager balloonStateManager = other.GetComponent<BalloonStateManager>();
        SetHammerBalloonForce(balloonStateManager, 0);
        if (balloonStateManager.GetState() != balloonStateManager.BalloonFlower) return;


        other.GetComponent<CharacterControllerTest>().SetForce(_lastForce, _stopLerpValue);
        _isPlayerIn = false;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}
