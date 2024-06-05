using BlownAway.Character;
using System;

namespace BlownAway.Collectibles
{
    public class CharacterCollectiblesManager : CharacterSubComponent
    {
        public Action OnCoinGain;
        public Action OnCoinRemove;
        public Action OnCoinGainPreview;
        public Action OnRareCollectibleGain;

        public int Coins { get; private set; }
        public int RareCollectibles { get; private set; }
        public int CurrentCoins { get; private set; }

        public int MaxCoins { get; private set; }
        public int MaxRareCollectibles { get; private set; }

        protected override void StartScript(CharacterManager manager)
        {
            base.StartScript(manager);
            GetMaxCollectibles();
        }

        public void AddCoin()
        {
            Coins++;
            CurrentCoins++;
            OnCoinGain?.Invoke();
        }

        public void RemoveCoin(int amount)
        {
            CurrentCoins = Math.Max(0, CurrentCoins - amount);
            OnCoinRemove?.Invoke();
        }

        public void AddRareCollectible()
        {
            RareCollectibles++;
            OnRareCollectibleGain?.Invoke();
        }

        public void AddCoinPreview()
        {
            OnCoinGainPreview?.Invoke();
        }

        private void GetMaxCollectibles()
        {
            int coinsCount = FindObjectsOfType<Coin>().Length;
            int rareCollectibleCount = FindObjectsOfType<RareCollectible>().Length;

            var bigCoins = FindObjectsOfType<BigCoin>();
            int bigCoinsCount = bigCoins.Length;
            var bigCoinsValue = bigCoinsCount > 0 ? bigCoins[0].CoinsNumber : 1;

            MaxCoins = coinsCount + bigCoinsCount * bigCoinsValue;
            MaxRareCollectibles = rareCollectibleCount;
        }
    }
}