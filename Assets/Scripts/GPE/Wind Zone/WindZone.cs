using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    [SerializeField] private Vector3 _pushVector;
    [SerializeField] private float _airPercentageAddedOnContact;
    [SerializeField] private float _timeBetweenAdditions;
    [SerializeField][Range(0, 1)] private float _startLerpValue;
    [SerializeField][Range(0, 1)] private float _stopLerpValue;
    private float _timer;

    private void Start()
    {
        _timer = _timeBetweenAdditions;
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

        other.GetComponent<CharacterControllerTest>().SetForce(_pushVector * Time.deltaTime, _startLerpValue);
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
