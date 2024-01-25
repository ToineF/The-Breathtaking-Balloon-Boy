using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class RadialUI : MonoBehaviour
{
    public float FillAmount
    {
        get => _fillAmount; protected set
        {
            _fillAmount = Mathf.Clamp(value, 0, 1);
            _filledImage.fillAmount = _fillAmount;
            CheckUIVisibilty();
        }
    }

    [SerializeField] private Image[] _allUIImages;
    [SerializeField] private Image _filledImage;
    

    [Header("Parameters")]
    [SerializeField] private bool _hideOnFull;
    [SerializeField] private float _appearSpeed;
    [SerializeField] private float _appearDelay;
    [SerializeField] private float _hideSpeed;
    [SerializeField] private float _hideDelay;
    [SerializeField] private Color _naturalColor;
    [SerializeField] private Color _fullColor;
    [SerializeField] private float _fullColorTime;
    [SerializeField] private float _fullColorDelay;
    [SerializeField] private float _fullToNaturalColorSpeed;

    private float _fillAmount;
    private bool _hidden;

    private void Start()
    {
        _filledImage.color = _naturalColor;
    }

    public void SetFillAmount(float value)
    {
        FillAmount = value;
    }

    public void AddFillAmount(float value)
    {
        FillAmount += value;
    }

    private void CheckUIVisibilty()
    {
        if (_hideOnFull && !_hidden && FillAmount == 1)
        {
            HideUI();
        }
        if (_hideOnFull && _hidden && FillAmount != 1)
        {
            UnhideUI();
        }
    }

    private void HideUI()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeColorOverTime(_filledImage, _fullColor, _naturalColor, _fullColorTime, _fullToNaturalColorSpeed, _fullColorDelay));

        foreach (var ui in _allUIImages)
        {
            StartCoroutine(FadeWithDelay(ui, 0, _hideSpeed, _hideDelay));
        }
        _hidden = true;
    }

    private void UnhideUI()
    {
        StopAllCoroutines();
        foreach (var ui in _allUIImages)
        {
            StartCoroutine(FadeWithDelay(ui, 1, _appearSpeed, _appearDelay));
        }
        _hidden = false;
    }

    private IEnumerator ChangeColorOverTime(Image image, Color color1, Color color2, float transitionTime, float colorTransitionSpeed, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        image.color = color1;
        yield return new WaitForSeconds(transitionTime);
        image.DOColor(color2, colorTransitionSpeed);
    }

    private IEnumerator FadeWithDelay(Image image, float fadeAlpha, float fadeSpeed, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        image.DOComplete();
        image.DOFade(fadeAlpha, fadeSpeed);
    }
}
