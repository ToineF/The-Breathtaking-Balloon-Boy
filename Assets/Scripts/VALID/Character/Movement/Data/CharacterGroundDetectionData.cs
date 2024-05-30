using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "GroundDetectionData", menuName = "CharacterData/MovementsData/Ground Detection")]
    public class CharacterGroundDetectionData : ScriptableObject
    {
        [field:Header("Ground Check")]
        [field: SerializeField] public float MinGroundCheckDistance { get; private set; }
        [field: SerializeField] public float MaxGroundCheckDistance { get; private set; }
        [field:SerializeField, Tooltip("The radius of the ground detection sphere collider")] public float GroundDetectionSphereRadius { get; private set; }
        [field:SerializeField, Tooltip("The layer of the ground the character can walk on")] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField, Tooltip("The distance the collider will aim to be at from the ground")] public float TargetDistanceFromGround { get; private set; }

        [field: Header("Jump Buffer")]
        [field:SerializeField, Tooltip("The distance offset of the jump buffer detection check from the character")] public float JumpBufferCheckDistance {  get; private set; }

    }
}