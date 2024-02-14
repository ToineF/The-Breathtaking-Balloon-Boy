using BlownAway.Character;
using UnityEngine;

namespace BlownAway
{
    public class GameManager : Singleton<GameManager>
    {
        // Character (Unused)
        [field: SerializeField] public CharacterManager CharacterManager { get; private set; }

    }
}