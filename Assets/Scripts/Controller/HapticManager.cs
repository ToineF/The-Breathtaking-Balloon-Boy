using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void VibrateForTime(float lowFrequency, float highFrequency, float time)
    {
        if (Gamepad.current == null) return;
        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
        StartCoroutine(VibrateController(time));
    }

    IEnumerator VibrateController(float time)
    {
        yield return new WaitForSeconds(time);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
