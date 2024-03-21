using UnityEngine;


namespace BlownAway.Character.Movements.Data {
    [CreateAssetMenu(fileName = "SlopesData", menuName = "CharacterData/MovementsData/Slopes")]
    public class CharacterSlopesData : ScriptableObject
    {
        [field: SerializeField] public float MaxSlopeAngle {get; private set;} 
        [field: SerializeField] public float SlopesGroundCheckDistance { get; private set;} 
        [field: SerializeField] public float CharacterColliderCheckOffset { get; private set;} 
        [field: SerializeField] public float MaxBounces { get; private set;} 
    }
}