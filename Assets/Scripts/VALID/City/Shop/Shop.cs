using BlownAway.Character;
using UnityEngine;

namespace BlownAway.City
{
    public class Shop : SubMenu
    {
        [SerializeField] private ShopItem[] _items;

        public void Open(int playerCoins)
        {
            OpenMenu(CanvasGroup, FirstSelectedButton);
            foreach (var item in _items)
            {
                item.UpdateUI(playerCoins);
            }
        }
    }
}