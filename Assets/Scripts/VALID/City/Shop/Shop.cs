using BlownAway.Character;
using BlownAway.Collectibles;
using UnityEngine;

namespace BlownAway.City
{
    public class Shop : SubMenu
    {
        [SerializeField] private ShopItem[] _items;

        private void Start()
        {
            foreach (var item in _items)
            {
                item.OnBuy += UpdateAllItems;
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
    }
}