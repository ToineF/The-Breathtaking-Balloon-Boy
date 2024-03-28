using UnityEngine;

public class CharacterCollectiblesManager : MonoBehaviour
{
    public float Coins { get; private set; }
    public float RareCollectibles { get; private set; }

    public void AddCoin()
    {
        Coins++;
    }

    public void AddRareCollectibles()
    {
        RareCollectibles++;
    }
}
