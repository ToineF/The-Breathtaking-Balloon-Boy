using UnityEngine;
using TMPro;
using BlownAway.Collectibles;
using System;

namespace BlownAway.City
{
    public class ShopItem : MonoBehaviour
    {
        public Action<CharacterCollectiblesManager> OnBuy;

        [SerializeField] private int _price;

        [Header("References")]
        [SerializeField] private TMP_Text _priceUI;
        [SerializeField] private Color _affordablePriceColor = Color.white;
        [SerializeField] private Color _overpricedColor = Color.red;

        private bool _isAffordable;
        private CharacterCollectiblesManager _player;


        private void Awake()
        {
            _priceUI.text = _price.ToString();
        }

        public void UpdateUI(CharacterCollectiblesManager player)
        {
            _player = player;
            _isAffordable = player.CurrentCoins >= _price;
            _priceUI.color = _isAffordable ? _affordablePriceColor : _overpricedColor;
        }

        public void TryBuy()
        {
            if (!_isAffordable) return;

            _player.RemoveCoin(_price);
            OnBuy?.Invoke(_player);
        }
    }
}