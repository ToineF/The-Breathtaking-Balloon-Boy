using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "LateralData", menuName = "MovementsData/Lateral")]
    public class CharacterLateralMovementsData : ScriptableObject
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
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while falling")] public float BaseFallLateralSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFallLateralSpeed) after pressing the inputs")] public float BaseFallTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFallLateralSpeed) after pressing the inputs")] public AnimationCurve BaseFallCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while falling")] public float FallDirectionTurnSpeed { get; set; }

        [field: Header("Float")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while floating")] public float BaseFloatLateralSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFloatLateralSpeed) after pressing the inputs")] public float BaseFloatTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFloatLateralSpeed) after pressing the inputs")] public AnimationCurve BaseFloatCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while floating")] public float FloatDirectionTurnSpeed { get; set; }

        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while propulsing")] public float BasePropulsionLateralDeplacementSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BasePropulsionLateralDeplacementSpeed) after pressing the inputs")] public float BasePropulsionDeplacementTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BasePropulsionLateralDeplacementSpeed) after pressing the inputs")] public AnimationCurve BasePropulsionDeplacementCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while propulsing")] public float PropulsionDirectionTurnSpeed { get; set; }

    }
}