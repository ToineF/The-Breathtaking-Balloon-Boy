using UnityEngine;
using TMPro;
using BlownAway.Collectibles;
using System;
using UnityEngine.UI;

namespace BlownAway.City
{
    public class ShopItem : MonoBehaviour
    {
        public Action<CharacterCollectiblesManager> OnBuy;

        [SerializeField] private int[] _prices;

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
            if (!_isAffordable) return;
            if (_soldOut) return;

            _player.RemoveCoin(_prices[_currentItem]);

            if (_currentItem >= _prices.Length - 1)
            {
                _soldOut = true;

            } else
            {
                _currentItem++;
            }

            OnBuy?.Invoke(_player);
        }
    }
}