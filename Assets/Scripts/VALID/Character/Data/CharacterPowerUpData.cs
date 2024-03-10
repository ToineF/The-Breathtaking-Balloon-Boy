using UnityEngine;

namespace BlownAway.Character.Data
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "CharacterData/Power Up")]
    public class CharacterPowerUpData : ScriptableObject
    {
        [field:SerializeField] public bool IsBalloonBounceAvailable { get; private set; }
        [field:SerializeField] public bool IsGroundPoundAvailable { get; private set; }

        [field:Header("Ground Pound")]
        [field:SerializeField] public float GroundPoundForce { get; private set; }
        [field:SerializeField, Range(0,1)] public float GroundPoundStartLerp { get; private set; }
        [field:SerializeField, Range(0, 1)] public float GroundPoundEndLerp { get; private set; }
    }
}