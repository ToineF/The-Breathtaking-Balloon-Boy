using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "PropulsionData", menuName = "MovementsData/Propulsion")]
    public class CharacterPropulsionData : ScriptableObject
    {
        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The maximum propulsion speed the character aims to moves at")] public float BasePropulsionSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BasePropulsionSpeed) after pressing the inputs")] public float BasePropulsionAccelTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BasePropulsionSpeed) after pressing the inputs")] public AnimationCurve BasePropulsionAccelCurve { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to stop moving after releasing the inputs")] public float BasePropulsionDecelTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to stop moving after releasing the inputs")] public AnimationCurve BasePropulsionDecelCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while propulsing")] public float PropulsionDirectionTurnSpeed { get; set; }
    }
}