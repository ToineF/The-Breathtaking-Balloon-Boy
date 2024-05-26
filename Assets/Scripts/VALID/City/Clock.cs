using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject _minutesNeedle;
    [SerializeField] private GameObject _hoursNeedle;
    [SerializeField] private Vector3 _rotationAxis;

    private Vector3 _minuteStartAngle;
    private Vector3 _hoursStartAngle;

    private void Start()
    {
        Debug.Log("HOURS: " + DateTime.Now.Hour);
        Debug.Log("MINUTES: " + DateTime.Now.Minute);
        _minuteStartAngle = _minutesNeedle.transform.localEulerAngles;
        _hoursStartAngle = _hoursNeedle.transform.localEulerAngles;
        UpdateRotation();
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float minuteAngle = DateTime.Now.Minute / 60f * 360;
        float hourAngle = (DateTime.Now.Hour % 12 / 12f + minuteAngle/360f/12f) * 360;
        Quaternion minutesRotation = Quaternion.Euler(_minuteStartAngle + _rotationAxis * -minuteAngle);
        Quaternion hoursRotation = Quaternion.Euler(_hoursStartAngle + _rotationAxis * -hourAngle);
        _minutesNeedle.transform.localRotation = minutesRotation;
        _hoursNeedle.transform.localRotation = hoursRotation;
    }
}
