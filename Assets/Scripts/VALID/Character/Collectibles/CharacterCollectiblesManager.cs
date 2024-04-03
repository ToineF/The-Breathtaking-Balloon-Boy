using BlownAway.Character;
using System;

namespace BlownAway.Collectibles
{
    public class CharacterCollectiblesManager : CharacterSubComponent
    {
        public Action OnCoinGain;
        public Action OnCoinGainPreview;
        public Action OnRareCollectibleGain;

        public float Coins { get; private set; }
        public float RareCollectibles { get; private set; }

        public void AddCoin()
        {
            Coins++;
            OnCoinGain?.Invoke();
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
    }
}