using BlownAway.Player;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class PhotographyZone : MonoBehaviour
{
    [SerializeField] private Image _pictureImage;
    [SerializeField] private float _pictureShowTime;
    [SerializeField] private float _pictureAppearTime;
    [SerializeField] private Ease _pictureAppearEase;
    [SerializeField] private float _pictureDisappearTime;
    [SerializeField] private Ease _pictureDisappearEase;

    private bool _pictureShown;
    private Vector3 _pictureScale;

    private void Start()
    {
        _pictureShown = false;
        _pictureScale = _pictureImage.transform.localScale;
        _pictureImage.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_pictureShown) return;
        if (!other.GetComponent<CharacterControllerTest>()) return;

        _pictureShown = true;

        StartCoroutine(ShowPictureToPlayer());
    }

    private IEnumerator ShowPictureToPlayer()
    {
        ShowPicture();
        yield return new WaitForSeconds(_pictureAppearTime);
        yield return new WaitForSeconds(_pictureShowTime);
        HidePicture();
    }

    private void ShowPicture()
    {
        _pictureImage.DOFade(1, _pictureAppearTime).SetEase(_pictureAppearEase);
        _pictureImage.transform.DOScale(_pictureScale, _pictureAppearTime).SetEase(_pictureAppearEase);
    }

    private void HidePicture()
    {
        _pictureImage.DOFade(0, _pictureDisappearTime).SetEase(_pictureDisappearEase);
        _pictureImage.transform.DOScale(Vector3.zero, _pictureDisappearTime).SetEase(_pictureDisappearEase);
    }


}
