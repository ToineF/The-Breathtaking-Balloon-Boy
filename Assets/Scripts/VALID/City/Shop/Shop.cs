using BlownAway.Character;
using BlownAway.Collectibles;
using TMPro;
using UnityEngine;

namespace BlownAway.City
{
    public class Shop : SubMenu
    {
        [SerializeField] private ShopItem[] _items;
        [SerializeField] private TMP_Text _itemDescription;

        private void Start()
        {
            foreach (var item in _items)
            {
                item.OnBuy += UpdateAllItems;
                item.OnSelectItem += UpdateDescription;
            }
        }

        public void Open(CharacterCollectiblesManager player)
        {
            OpenMenu(CanvasGroup, FirstSelectedButton);
            UpdateAllItems(player);
        }

        public void Close()
        {
            CloseMenu(CanvasGroup);
        }

        private void UpdateAllItems(CharacterCollectiblesManager player)
        {
            foreach (var item in _items)
            {
                item.UpdateUI(player);
            }
        }

        private void UpdateDescription(string description)
        {
            _itemDescription.text = description;
        }
    }
}