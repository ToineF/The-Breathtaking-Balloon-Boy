using UnityEngine;
using TMPro;

namespace BlownAway.City
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private int _price;

        [Header("References")]
        [SerializeField] private TMP_Text _priceUI;
        [SerializeField] private Color _affordablePriceColor = Color.white;
        [SerializeField] private Color _overpricedColor = Color.red;

        private void Awake()
        {
            _priceUI.text = _price.ToString();
            UpdateUI(0);
        }

        public void UpdateUI(int playerCoins)
        {
            bool isAffordable = playerCoins >= _price;
            _priceUI.color = isAffordable ? _affordablePriceColor : _overpricedColor;
        }

        public void TryBuy()
        {

        }
    }
}