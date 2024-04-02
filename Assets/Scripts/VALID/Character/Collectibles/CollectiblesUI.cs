using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AntoineFoucault.Utilities;

namespace BlownAway.Collectibles
{
    public class CollectiblesUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterCollectiblesManager _collectiblesManager;
        [SerializeField] private CharacterChildrenManager _childrenManager;

        [SerializeField] private CanvasGroup _collectiblesUI;

        [Header("Children")]
        [SerializeField] private Transform _childrenUIParent;
        [SerializeField] private Image _childHiddenImagePrefab;
        [SerializeField] private Image _childFoundImagePrefab;

        [Header("Coin")]
        [SerializeField] private CanvasGroup _coinsUI;
        [SerializeField] private TMP_Text _coinsCountText;

        [Header("Rare Collectible")]
        [SerializeField] private CanvasGroup _rareCollectibleUI;
        [SerializeField] private TMP_Text _rareCollectibleCountText;

        private Image[] _collectiblesImage;

        private void Start()
        {
            _collectiblesManager.OnCoinGain += UpdateCoinUI;
            _collectiblesManager.OnRareCollectibleGain += UpdateRareCollectibleUI;
            _childrenManager.OnChildGain += UpdateChildrenUI;

            UpdateCoinUI();
            UpdateRareCollectibleUI();
            CreateChildrenUI();
        }


        private void UpdateCoinUI()
        {
            _coinsCountText.text = _collectiblesManager.Coins.ToString();
        }
        private void UpdateRareCollectibleUI()
        {
            _rareCollectibleCountText.text = _collectiblesManager.RareCollectibles.ToString();
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
        }
        private void CreateChildrenUI()
        {
            _collectiblesImage = new Image[_childrenManager.MaxChildrenCount];

            for (int i = 0; i < _childrenManager.MaxChildrenCount; i++)
            {
                _collectiblesImage[i] = Instantiate(_childHiddenImagePrefab, _childrenUIParent);
            }
        }
    }
}