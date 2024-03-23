using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject _minutesNeedle;
    [SerializeField] private GameObject _hoursNeedle;

    private void Start()
    {
        Debug.Log("HOURS: " + DateTime.Now.Hour);
        Debug.Log("MINUTES: " + DateTime.Now.Minute);
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
        Quaternion minutesRotation = Quaternion.Euler(new Vector3(0, -minuteAngle, 0));
        Quaternion hoursRotation = Quaternion.Euler(new Vector3(0, -hourAngle, 0));
        _minutesNeedle.transform.localRotation = minutesRotation;
        _hoursNeedle.transform.localRotation = hoursRotation;
    }
}
