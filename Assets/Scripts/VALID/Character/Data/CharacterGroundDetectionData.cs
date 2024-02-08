using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "GroundDetectionData", menuName = "MovementsData/Ground Detection")]
    public class CharacterGroundDetectionData : ScriptableObject
    {
        [Header("Ground Check")]
        [Tooltip("The distance offset of the ground detection check from the character")] public float GroundCheckDistance;
        [Tooltip("The radius of the ground detection sphere collider")] public float GroundDetectionSphereRadius;
        [Tooltip("The layer of the ground the character can walk on")] public LayerMask GroundLayer;

        [field: Header("Jump Buffer")]
        [Tooltip("The distance offset of the jump buffer detection check from the character")] public float JumpBufferCheckDistance;

    }
}