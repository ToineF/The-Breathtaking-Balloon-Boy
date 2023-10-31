using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class WindZone : MonoBehaviour
{
    [SerializeField][Tooltip("The magnitude of the applied force, as a positive number")] private float _forceMagnitude;
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

        other.GetComponent<CharacterControllerTest>().SetForce(_forceMagnitude * Time.deltaTime);
        AirManager.Instance.AddAir(_airPercentageAddedOnContact);
        _timer = _timeBetweenAdditions;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterControllerTest>() == null) return;

        if (Mathf.Abs(other.GetComponent<CharacterControllerTest>().Force) > 0.1f) return;
        other.GetComponent<CharacterControllerTest>().SetForce(0);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }
}
