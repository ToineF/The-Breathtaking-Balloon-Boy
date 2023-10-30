using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirManager : MonoBehaviour
{
    private float _currentAir = 1;
    [SerializeField] private Image _currentAirGauge;

    public void SetCurrentAir(float percentage)
    {
        _currentAir = percentage / 100;
        _currentAir = Mathf.Clamp(_currentAir, 0, 1);

    }

    public void AddAir(float percentage)
    {
        _currentAir += percentage / 100;
        _currentAir = Mathf.Clamp(_currentAir, 0, 1);
    }

    private void Update()
    {
        _currentAirGauge.fillAmount = _currentAir;

        if (Input.GetKeyDown(KeyCode.K)) AddAir(-10);
    }
}
