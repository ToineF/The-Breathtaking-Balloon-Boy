using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class SliderText : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [Header("Settings")]
    [SerializeField] private bool _normalizeText = true;
    [SerializeField] private float _normalizedMultiplier = 100f;
    [SerializeField] private float _maxDecimalsShown = 0f;
    private TMP_Text _text;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _slider.onValueChanged.AddListener(UpdateText);
    }

    private void Start()
    {
        UpdateText(_slider.value);
    }

    private void UpdateText(float value)
    {
        if (_normalizeText)
        {
            value = (value - _slider.minValue) / (_slider.maxValue - _slider.minValue);
            value *= _normalizedMultiplier;
        }

        string formatText = GetFormatedText();

        _text.text = String.Format(formatText, value);
    }

    private string GetFormatedText()
    {
        string formatText = "{0:0";
        for (int i = 0; i < _maxDecimalsShown; i++)
        {
            if (i == 0) formatText += '.';
            formatText += '#';
        }
        formatText += '}';
        return formatText;
    }
}
