using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "PropulsionData", menuName = "CharacterData/MovementsData/Propulsion")]
    public class CharacterPropulsionData : ScriptableObject
    {
        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The vertical propulsion speed multiplier the character aims to moves at")] public float VerticalPropulsionMultiplier {get; private set;}
        [field: SerializeField, Tooltip("The horizontal propulsion speed multiplier the character aims to moves at")] public float HorizontalPropulsionMultiplier {get; private set;}
        [field: SerializeField, Tooltip("The maximum propulsion speed the character can move at")] public float MaxPropulsionSpeed { get; private set;}
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed after pressing the inputs")] public float BasePropulsionAccelTime {get; private set;}
        [field: SerializeField, Tooltip("The lerp used by the character to reach the maximum speed after pressing the inputs")] public AnimationCurve BasePropulsionAccelCurve {get; private set;}
        [field: SerializeField, Tooltip("The time it takes to the character to stop moving after releasing the inputs")] public float BasePropulsionDecelTime {get; private set;}
        [field: SerializeField, Tooltip("The lerp used by the character to stop moving after releasing the inputs")] public AnimationCurve BasePropulsionDecelCurve {get; private set;}
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while propulsing")] public float PropulsionDirectionTurnSpeed {get; private set;}
        [field: SerializeField, Tooltip("The increase of propulsion speed added at each frame when propulsing")] public float PropulsionIncreaseByFrame { get; private set;}
        [field: SerializeField, Tooltip("The deceleration of the increase of propulsion speed added at each frame when propulsing")] public float PropulsionIncreaseDeceleration { get; private set;}
        [field: SerializeField, Tooltip("The minimum time the character stays in the propulsion state")] public float MinimumPropulsionTime { get; private set;}
    
        [field:Header("Take off")]
        [field: SerializeField, Tooltip("The maximum propulsion speed the character's initial take-off aims to moves at")] public float PropulsionTakeOffSpeed {get; private set;}
        [field: SerializeField, Tooltip("The time it takes to the propulsion take-off to reach its maximum value")] public float PropulsionTakeOffAccelTime {get; private set;}
        [field: SerializeField, Tooltip("The lerp used by the propulsion take-off to reach its maximum value")] public AnimationCurve PropulsionTakeOffAccelCurve {get; private set;}
        [field: SerializeField, Tooltip("The time it takes to the propulsion take-off to stop moving after reaching its maximum value")] public float PropulsionTakeOffDecelTime {get; private set;}
        [field: SerializeField, Tooltip("The lerp used by the propulsion take-off to stop moving after reaching its maximum value")] public AnimationCurve PropulsionTakeOffDecelCurve {get; private set;}

        [field:Header("Derive")]
        [field: SerializeField, Tooltip("The maximum time the derive lasts")] public float DeriveTime { get; private set; }

    }
}