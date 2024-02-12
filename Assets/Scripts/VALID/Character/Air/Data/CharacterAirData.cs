using UnityEngine;

namespace BlownAway.Character.Air.Data
{
    [CreateAssetMenu(fileName = "Air Data", menuName = "CharacterData/Air")]
    public class CharacterAirData : ScriptableObject
    {
        [field: SerializeField, Tooltip("The speed at which the air reduces while floating")] public float FloatingAirReductionSpeed { get; private set; }
        [field: SerializeField, Tooltip("The speed at which the air reduces while propulsing")] public float PropulsionAirReductionSpeed { get; private set; }
        [field: SerializeField, Tooltip("The speed at which the air refills while falling")] public float FallingAirRefillSpeed { get; private set; }
        [field: SerializeField, Tooltip("The delay at which the air refills while falling")] public float FallingAirRefillDelay { get; private set; }
    }
}