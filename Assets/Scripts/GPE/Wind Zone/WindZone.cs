using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    [SerializeField] private float _pushMagnitude;
    [SerializeField] private Vector3 _pushVector;
    [SerializeField] private bool _isHot;
    [SerializeField] private float _airPercentageAddedOnContact;
    [SerializeField] private float _timeBetweenAdditions;
    [SerializeField] [Range(0, 1)] private float _startLerpValue;
    [SerializeField] [Range(0, 1)] private float _stopLerpValue;
    private float _timer;

    [Header("FX")]
    [SerializeField] private GameObject _FXWind;
    [SerializeField] private ParticleSystem[] _FXWinds;
    [SerializeField] private Color _HotColor;
    [SerializeField] private Color _ColdColor;

    private void Start()
    {
        _timer = _timeBetweenAdditions;

        var fxWindZ = _pushVector.x > 0 ? -90 : _pushVector.x < 0 ? 90 : 0;
        var fxWindX = _pushVector.z < 0 ? -90 : _pushVector.z > 0 ? 90 : 0;
        fxWindZ = _pushVector.y < 0 ? 180 : fxWindZ;
        _FXWind.transform.eulerAngles = new Vector3(fxWindX, 0, fxWindZ);

        foreach (var fx in _FXWinds)
        {
            Color color = _isHot ? _HotColor : _ColdColor;
            fx.startColor = color;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (_timer > 0) return;

        if (other.GetComponent<CharacterControllerTest>() == null) return;

        AirManager.Instance.AddAir(_airPercentageAddedOnContact);
        _timer = _timeBetweenAdditions;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterControllerTest>() == null) return;

        other.GetComponent<CharacterControllerTest>().SetForce(_pushVector * _pushMagnitude * Time.deltaTime, _startLerpValue);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterControllerTest>() == null) return;

        other.GetComponent<CharacterControllerTest>().SetForce(Vector3.zero, _stopLerpValue);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}
