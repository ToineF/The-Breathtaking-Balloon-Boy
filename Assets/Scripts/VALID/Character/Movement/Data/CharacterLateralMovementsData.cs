using UnityEngine;
using System;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "LateralData", menuName = "CharacterData/MovementsData/Lateral")]
    public class CharacterLateralMovementsData : ScriptableObject
    {
        [field: Header("Idle")]
        [field: SerializeField] public LateralData IdleData { get; private set; }

        [field:Header("Walk")]
        [field: SerializeField] public LateralData WalkData { get; private set; }

        [field: Header("Fall")]
        [field: SerializeField] public LateralData FallingData { get; private set; }

        [field: Header("Float")]
        [field: SerializeField] public LateralData FloatingData { get; private set; }

        [field: Header("Propulsion")]
        [field: SerializeField] public LateralData PropulsionData { get; private set; }

        [field: Header("Jump")]
        [field: SerializeField] public LateralData JumpData { get; private set; }

        [field: Header("Ground Pound")]
        [field: SerializeField] public LateralData GroundPoundData { get; private set; }

    }

    [Serializable]
    public struct LateralData
    {
        [field: SerializeField, Tooltip("The maximum deplacement speed the character aims to moves at")] public float DeplacementLateralSpeed { get; private set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (DeplacementLateralSpeed) after pressing the inputs")] public float DeplacementTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (DeplacementLateralSpeed) after pressing the inputs")] public AnimationCurve DeplacementCurve { get; private set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction")] public float DirectionTurnSpeed { get; private set; }
    }
}