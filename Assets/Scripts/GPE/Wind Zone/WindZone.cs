using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    [SerializeField] private float _airPercentageAddedOnContact;
    [SerializeField] private float _timeBetweenAdditions;
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

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}
