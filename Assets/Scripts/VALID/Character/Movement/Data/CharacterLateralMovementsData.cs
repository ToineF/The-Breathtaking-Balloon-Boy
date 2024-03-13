using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "LateralData", menuName = "CharacterData/MovementsData/Lateral")]
    public class CharacterLateralMovementsData : ScriptableObject
    {
        [field: Header("Idle")]
        [field: SerializeField, Tooltip("The time it takes to the character to stop moving after releasing the inputs")] public float BaseIdleTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to stop moving after releasing the inputs")] public AnimationCurve BaseIdleCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while not moving")] public float IdleDirectionTurnSpeed { get; private set; }

        [field:Header("Walk")]
        [field: SerializeField, Tooltip("The maximum walking speed the character aims to moves at")] public float BaseWalkSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public float BaseWalkTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public AnimationCurve BaseWalkCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while walking")] public float WalkDirectionTurnSpeed { get; private set; }

        [field: Header("Fall")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while falling")] public float BaseFallLateralSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFallLateralSpeed) after pressing the inputs")] public float BaseFallTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFallLateralSpeed) after pressing the inputs")] public AnimationCurve BaseFallCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while falling")] public float FallDirectionTurnSpeed { get; private set; }

        [field: Header("Float")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while floating")] public float BaseFloatLateralSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseFloatLateralSpeed) after pressing the inputs")] public float BaseFloatTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseFloatLateralSpeed) after pressing the inputs")] public AnimationCurve BaseFloatCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while floating")] public float FloatDirectionTurnSpeed { get; private set; }

        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while propulsing")] public float BasePropulsionLateralDeplacementSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BasePropulsionLateralDeplacementSpeed) after pressing the inputs")] public float BasePropulsionDeplacementTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BasePropulsionLateralDeplacementSpeed) after pressing the inputs")] public AnimationCurve BasePropulsionDeplacementCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while propulsing")] public float PropulsionDirectionTurnSpeed { get; private set; }

        [field: Header("Ground Pound")]
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at while ground pounding")] public float BaseGroundPoundFallLateralSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseDeriveLateralSpeed) after pressing the inputs")] public float GroundPoundFallTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseDeriveLateralSpeed) after pressing the inputs")] public AnimationCurve GroundPoundFallCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction while ground pounding")] public float GroundPoundDirectionTurnSpeed { get; private set; }

    }
}