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
                item.OnBuy.AddListener(UpdateAllItems);
                item.OnSelectItem += UpdateDescription;
            }
        }

        public void Open(CharacterCollectiblesManager player)
        {
            OpenMenu(this, FirstSelectedButton);
            UpdateAllItems(player);
            CanvasGroup.gameObject.SetActive(true);
        }

        public void Close()
        {
            CloseMenu(this);
            CanvasGroup.gameObject.SetActive(false);
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