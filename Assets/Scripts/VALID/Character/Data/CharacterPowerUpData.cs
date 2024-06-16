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
        [field: SerializeField, Tooltip("The speed of the dash")] public float DashStartSpeed { get; private set; }
        [field: SerializeField, Tooltip("The speed of the dash when the dash ends")] public float DashEndSpeed { get; private set; }
        [field: SerializeField, Tooltip("The curve of the interpolation between DashSpeed and DashEndSpeed")] public AnimationCurve DashInterpolationCurve { get; private set; }
        [field: SerializeField, Tooltip("Does dashing empty the player's air")] public bool DashEmptiesAir { get; private set; }
        [field: SerializeField, Tooltip("Numbers of dashes the player can do")] public int MaxDashes { get; private set; }
        public int BonusMaxDashes { get; set; }

        [field:Header("Ground Pound")]
        [field:SerializeField, Tooltip("The upper force of the ground pound when touching the ground")] public float GroundPoundNormalForce { get; private set; }
        [field:SerializeField, Tooltip("The upper force of the ground pound when touching the ground from a small distance")] public float GroundPoundSmallForce { get; private set; }
        [field:SerializeField, Tooltip("The upper force of the ground pound when touching a balloon")] public float GroundPoundBalloonForce { get; private set; }
        [field:SerializeField, Range(0,1), Tooltip("The lerp of the ground pound start")] public float GroundPoundStartLerp { get; private set; }
        [field:SerializeField, Range(0, 1), Tooltip("The lerp of the ground pound end")] public float GroundPoundEndLerp { get; private set; }
        [field:SerializeField, Tooltip("The time before the ground pound ends")] public float GroundPoundEndTime { get; private set; }
        [field:SerializeField, Tooltip("The minimum distance for a ground to be considered (GroundPoundNormalForce)")] public float GroundPoundNormalGroundHeightThresold { get; private set; }
        [field:SerializeField, Tooltip("The minimum time the player is locked in the ground pound state before allowing to cancel it")] public float MinGroundPoundCancelTime { get; private set; }
    }
}