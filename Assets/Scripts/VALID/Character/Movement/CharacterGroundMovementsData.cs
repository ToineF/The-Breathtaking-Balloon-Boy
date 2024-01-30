using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "CharacterGroundMovementsData", menuName = "MovementsData/Ground")]
    public class CharacterGroundMovementsData : ScriptableObject
    {
        [field: Header("Idle")]
        [field: SerializeField, Tooltip("The time it takes to the character to stop moving after releasing the inputs")] public float BaseIdleTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to stop moving after releasing the inputs")] public AnimationCurve BaseIdleCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while not moving")] public float IdleDirectionTurnSpeed { get; set; }

        [field:Header("Walk")]
        [field: SerializeField, Tooltip("The maximum walking speed the character aims to moves at")] public float BaseWalkSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public float BaseWalkTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public AnimationCurve BaseWalkCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while walking")] public float WalkDirectionTurnSpeed { get; set; }

        [field: Header("Fall")]
        [field: SerializeField, Tooltip("The maximum walking speed the character aims to moves at")] public float BaseFallSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFallSpeed) after pressing the inputs")] public float BaseFallTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFallSpeed) after pressing the inputs")] public AnimationCurve BaseFallCurve { get; set; }
        //
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while walking")] public float FallDirectionTurnSpeed { get; set; }

        [field: Header("Float")]
        [field: SerializeField, Tooltip("The maximum walking speed the character aims to moves at")] public float BaseFloatSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFloatSpeed) after pressing the inputs")] public float BaseFloatTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFloatSpeed) after pressing the inputs")] public AnimationCurve BaseFloatCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while walking")] public float FloatDirectionTurnSpeed { get; set; }

    }
}