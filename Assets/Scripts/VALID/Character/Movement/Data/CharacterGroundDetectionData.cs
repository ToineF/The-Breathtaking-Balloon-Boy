using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "GroundDetectionData", menuName = "CharacterData/MovementsData/Ground Detection")]
    public class CharacterGroundDetectionData : ScriptableObject
    {
        [field:Header("Ground Check")]
        [field:SerializeField, Tooltip("The gravity to use while touching the ground to make sure the character rigidbody isn't offsetted from the ground")] public float GroundedGravity { get; private set; }
        [field:SerializeField, Tooltip("The distance offset of the ground detection check from the character")] public float GroundCheckDistance { get; private set; }
        [field:SerializeField, Tooltip("The radius of the ground detection sphere collider")] public float GroundDetectionSphereRadius { get; private set; }
        [field:SerializeField, Tooltip("The layer of the ground the character can walk on")] public LayerMask GroundLayer { get; private set; }

        [field: Header("Jump Buffer")]
        [field:SerializeField, Tooltip("The distance offset of the jump buffer detection check from the character")] public float JumpBufferCheckDistance {  get; private set; }

        [field: Header("Slopes")]
        [field: SerializeField] public float MaxSlopeAngle { get; private set; }
        [field: SerializeField] public float SlopesGroundCheckDistance { get; private set; }
        [field: SerializeField] public float CharacterColliderCheckOffset { get; private set; }
        [field: SerializeField] public float MaxBounces { get; private set; }

        [field: Header("Stairs")]
        [field: SerializeField] public float StepHeight { get; private set; }
        [field: SerializeField] public float StepSmooth { get; private set; }
        [field: SerializeField] public float LowerRaycastLength { get; private set; } = 0.1f;
        [field: SerializeField] public float UpperRaycastLength { get; private set; } = 0.2f;

    }
}