using UnityEngine;

namespace BlownAway.Character.Data
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "CharacterData/Power Up")]
    public class CharacterPowerUpData : ScriptableObject
    {
        [field:SerializeField] public bool IsBalloonBounceAvailable { get; private set; }
        [field:SerializeField] public bool IsGroundPoundAvailable { get; private set; }
    }
}