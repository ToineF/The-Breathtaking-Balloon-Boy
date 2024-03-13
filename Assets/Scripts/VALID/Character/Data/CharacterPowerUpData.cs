using UnityEngine;

namespace BlownAway.Character.Data
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "CharacterData/Power Up")]
    public class CharacterPowerUpData : ScriptableObject
    {
        [field:SerializeField] public bool IsBalloonBounceAvailable { get; private set; }
        [field:SerializeField] public bool IsGroundPoundAvailable { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField, Tooltip("The time it takes for the dash to end")] public float DashDuration { get; private set; }
        [field: SerializeField, Tooltip("The speed of the dash")] public float DashSpeed { get; private set; }
        [field: SerializeField, Tooltip("The speed of the dash when the dash ends")] public float DashEndSpeed { get; private set; }
        [field: SerializeField, Tooltip("The maximum speed of the dash")] public float MaxDashSpeed { get; private set; }
        [field: SerializeField, Tooltip("Does dashing empty the player's air")] public bool DashEmptiesAir { get; private set; }
        [field: SerializeField, Tooltip("Numbers of dashes the player can do")] public float MaxDashes { get; private set; }

        [field:Header("Ground Pound")]
        [field:SerializeField] public float GroundPoundForce { get; private set; }
        [field:SerializeField, Range(0,1)] public float GroundPoundStartLerp { get; private set; }
        [field:SerializeField, Range(0, 1)] public float GroundPoundEndLerp { get; private set; }
    }
}