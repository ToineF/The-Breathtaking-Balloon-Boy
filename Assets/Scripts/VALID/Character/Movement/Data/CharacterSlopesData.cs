using UnityEngine;


namespace BlownAway.Character.Movements.Data {
    [CreateAssetMenu(fileName = "SlopesData", menuName = "CharacterData/MovementsData/Slopes")]
    public class CharacterSlopesData : ScriptableObject
    {
        [field: SerializeField] public float MaxSlopeAngle {get; private set;} 
        [field: SerializeField] public float SlopesGroundCheckDistance { get; private set;} 
        [field: SerializeField] public float CharacterColliderCheckOffset { get; private set;} 
        [field: SerializeField] public float MaxBounces { get; private set;}

        [field: Header("Stairs")]
        [field: SerializeField] public float StepHeight { get; private set; }
        [field: SerializeField] public float StepSmooth { get; private set; }
        [field: SerializeField] public float LowerRaycastLength { get; private set; } = 0.1f;
        [field: SerializeField] public float UpperRaycastLength { get; private set; } = 0.2f;
    }
}