using UnityEngine;
using TMPro;
using BlownAway.Collectibles;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BlownAway.City
{
    public class ShopItem : MonoBehaviour, ISelectHandler
    {
        public UnityEvent<CharacterCollectiblesManager> OnBuy;
        public UnityEvent<CharacterCollectiblesManager> OnBuyFirst;
        public UnityEvent<CharacterCollectiblesManager> OnBuyFail;
        public Action<string> OnSelectItem;

        [SerializeField] private int[] _prices;
        [SerializeField, TextArea] private string _description;

        [Header("References")]
        [SerializeField] private GameObject _priceUI;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Color _affordablePriceColor = Color.white;
        [SerializeField] private Color _overpricedColor = Color.red;
        [SerializeField] private Color _soldOutColor = Color.red;
        [SerializeField] private Image _soldOutImage;

        private bool _isAffordable;
        private CharacterCollectiblesManager _player;
        private int _currentItem = 0;
        private bool _soldOut = false;


        private void Awake()
        {
            _priceText.text = _prices[0].ToString();
        }

        public void UpdateUI(CharacterCollectiblesManager player)
        {
            _player = player;
            if (!_soldOut)
            {
                _isAffordable = player.CurrentCoins >= _prices[_currentItem];
                _priceText.color = _isAffordable ? _affordablePriceColor : _overpricedColor;
                _priceText.text = _prices[_currentItem].ToString();
            } 
            else
            {
                _priceText.color = _soldOutColor;
                _priceText.text = "SOLD OUT";
                _priceUI.SetActive(false);
                _soldOutImage.gameObject.SetActive(true);
            }
        }

        public void TryBuy()
        {
            if (!_isAffordable || _soldOut)
            {
                OnBuyFail?.Invoke(_player);
                return;
            }

            _player.RemoveCoin(_prices[_currentItem]);

            if (_currentItem == 0) OnBuyFirst?.Invoke(_player);

            if (_currentItem >= _prices.Length - 1)
            {
                _soldOut = true;

            } else
            {
                _currentItem++;
            }

            OnBuy?.Invoke(_player);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelectItem?.Invoke(_description);
        }
    }
}