using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static AntoineFoucault.Utilities.Tween;
using System.Collections;
using System;

namespace BlownAway.Collectibles
{
    public class CollectiblesUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterCollectiblesManager _collectiblesManager;
        [SerializeField] private CharacterChildrenManager _childrenManager;

        [SerializeField] private CanvasGroup _collectiblesUI;

        [Header("Visibility")]
        [SerializeField] private float _hideTime;
        [SerializeField] private float _appearFadeTime;
        [SerializeField] private float _disappearFadeTime;
        [SerializeField] private float _stayFadeTime;

        [Header("Children")]
        [SerializeField] private Transform _childrenUIParent;
        [SerializeField] private Image _childHiddenImagePrefab;
        [SerializeField] private Image _childFoundImagePrefab;

        [Header("Coin")]
        [SerializeField] private CanvasGroup _coinsUI;
        [SerializeField] private TMP_Text _coinsCountText;
        [SerializeField] private TMP_Text _currentCoinsCountText;
        [SerializeField] private TMP_Text _maxCoinsCountText;

        [Header("Rare Collectible")]
        [SerializeField] private CanvasGroup _rareCollectibleUI;
        [SerializeField] private TMP_Text _rareCollectibleCountText;
        [SerializeField] private TMP_Text _maxRareCollectibleCountText;

        [Header("Feedbacks")]
        [SerializeField] private DoTweenPunchFeedback _coinFeedbacks;
        [SerializeField] private DoTweenPunchFeedback _rareCollectibleFeedbacks;

        private Image[] _collectiblesImage;
        private Coroutine _currentVisibilityCoroutine;
        private bool _isVisible;
        private bool _highPriority;

        private void Start()
        {
            _collectiblesManager.OnCoinGain += UpdateCoinUI;
            _collectiblesManager.OnCoinRemove += UpdateCoinUI;
            _collectiblesManager.OnCoinGainPreview += ShowUICollectible;
            _collectiblesManager.OnRareCollectibleGain += UpdateRareCollectibleUI;
            _childrenManager.OnChildGain += UpdateChildrenUI;

            UpdateCoinUI();
            UpdateRareCollectibleUI();
            CreateChildrenUI();
            UpdateMaxCollectiblesUI();
        }

        private void UpdateMaxCollectiblesUI()
        {
            if (_maxCoinsCountText != null) _maxCoinsCountText.text = _collectiblesManager.MaxCoins.ToString();
            if (_maxRareCollectibleCountText != null) _maxRareCollectibleCountText.text = _collectiblesManager.MaxRareCollectibles.ToString();
        }

        private void UpdateCoinUI()
        {
            if (_coinsCountText != null)
            {
                _coinsCountText.transform.DOComplete();
                _coinsCountText.transform.DOPunchPosition(_coinFeedbacks.PunchDirection, _coinFeedbacks.PunchTime, _coinFeedbacks.PunchVibrato, _coinFeedbacks.PunchElasticity);
                _coinsCountText.text = _collectiblesManager.Coins.ToString();
            }

            if (_currentCoinsCountText != null)
            {
                _currentCoinsCountText.transform.DOComplete();
                _currentCoinsCountText.transform.DOPunchPosition(_coinFeedbacks.PunchDirection, _coinFeedbacks.PunchTime, _coinFeedbacks.PunchVibrato, _coinFeedbacks.PunchElasticity);
                _currentCoinsCountText.text = _collectiblesManager.CurrentCoins.ToString();
            }
        }
        private void UpdateRareCollectibleUI()
        {
            if (_rareCollectibleCountText == null) return;

            _rareCollectibleCountText.transform.DOComplete();
            _rareCollectibleCountText.transform.DOPunchPosition(_rareCollectibleFeedbacks.PunchDirection, _rareCollectibleFeedbacks.PunchTime, _rareCollectibleFeedbacks.PunchVibrato, _rareCollectibleFeedbacks.PunchElasticity);
            _rareCollectibleCountText.text = _collectiblesManager.RareCollectibles.ToString();
            ShowUICollectible();
        }
        private void UpdateChildrenUI()
        {
            for (int i = 0; i < _childrenManager.ChildrenCount; i++)
            {
                Destroy(_collectiblesImage[i].gameObject);
                _collectiblesImage[i] = Instantiate(_childFoundImagePrefab, _childrenUIParent);
            }
            for (int i = _childrenManager.ChildrenCount; i < _childrenManager.MaxChildrenCount; i++)
            {
                Destroy(_collectiblesImage[i].gameObject);
                _collectiblesImage[i] = Instantiate(_childHiddenImagePrefab, _childrenUIParent);
            }
            ShowUICollectible();
        }
        private void CreateChildrenUI()
        {
            _collectiblesImage = new Image[_childrenManager.MaxChildrenCount];

            for (int i = 0; i < _childrenManager.MaxChildrenCount; i++)
            {
                _collectiblesImage[i] = Instantiate(_childHiddenImagePrefab, _childrenUIParent);
            }
        }

        private void Update()
        {
            CheckForUIHide();
        }

        private void CheckForUIHide()
        {
            if (_childrenManager.Manager.States.IsInState(_childrenManager.Manager.States.IdleState)) // Stop moving
            {
                ShowHideUIAfterTime(_hideTime, true);
            }
            else if (!_childrenManager.Manager.States.IsInState(_childrenManager.Manager.States.IdleState)) // Starts moving
            {
                if (_currentVisibilityCoroutine != null) ShowHideUIAfterTime(0, false);
            }
        }

        private void ShowUICollectible()
        {
            _highPriority = true;
            ShowHideUIImmediate(true, true);
            ShowHideUIAfterTime(_stayFadeTime, false, true);
        }

        private void ShowHideUIAfterTime(float time, bool isVisible, bool highPriority = false)
        {
            if (isVisible == _isVisible && !highPriority) return;
            if (_highPriority && !highPriority) return;
            _isVisible = isVisible;

            if (_currentVisibilityCoroutine != null) StopCoroutine(_currentVisibilityCoroutine);
            _currentVisibilityCoroutine = StartCoroutine(ShowHideUIAfterTimeRoutine(time, isVisible));
        }

        private void ShowHideUIImmediate(bool isVisible, bool highPriority = false)
        {
            if (isVisible == _isVisible && !highPriority) return;
            if (_highPriority && !highPriority) return;
            _isVisible = isVisible;

            _collectiblesUI.DOFade(isVisible ? 1 : 0, isVisible ? _appearFadeTime : _disappearFadeTime);
        }

        private IEnumerator ShowHideUIAfterTimeRoutine(float time, bool isVisible)
        {
            yield return new WaitForSeconds(time);

            _collectiblesUI.DOFade(isVisible ? 1 : 0, isVisible ? _appearFadeTime : _disappearFadeTime);
            _highPriority = false;
        }
    }
}

