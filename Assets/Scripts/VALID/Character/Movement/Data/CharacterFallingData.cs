using System;
using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "FallingData", menuName = "CharacterData/MovementsData/Fall")]
    public class CharacterFallingData : ScriptableObject
    {
        [field: Header("Base")]
        [field: SerializeField] public FallData BaseData { get;  private set; }

        [field: Header("Floating")]
        [field: SerializeField] public FallData FloatingData { get; private set; }

        [field: Header("Propulsion")]
        [field: SerializeField] public FallData PropulsionData { get; private set; }

        [field: Header("Jump")]
        [field: SerializeField] public FallData JumpAscentData { get; private set; }
        [field: SerializeField] public FallData JumpDescentData { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField] public FallData DashData { get; private set; }

        [field: Header("Ground Pound")]
        [field: SerializeField] public FallData GroundPoundData { get; private set; }

        [field: Header("Grounded")]
        [field: SerializeField] public FallData GroundedData { get;  private set; }

    }

    [Serializable]
    public struct FallData
    {
        [field: SerializeField, Tooltip("The gravity the character falls at")] public float BaseGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at")] public float MinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at")] public float MaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (BaseGravity)")] public float GravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (BaseGravity)")] public AnimationCurve GravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when falling")] public float GravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when falling")] public float GravityIncreaseDecelerationByFrame { get; private set; }
    }
}
