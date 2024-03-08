using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "FallingData", menuName = "CharacterData/MovementsData/Fall")]
    public class CharacterFallingData : ScriptableObject
    {
        [field: Header("Base")]
        [field: SerializeField, Tooltip("The gravity the character falls at while not floating")] public float BaseGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while not floating")] public float BaseMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while not floating")] public float BaseMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (BaseGravity)")] public float BaseGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (BaseGravity)")] public AnimationCurve BaseGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when falling")] public float BaseGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when falling")] public float BaseGravityIncreaseDecelerationByFrame { get; private set; }

        [field: Header("Floating")]
        [field: SerializeField, Tooltip("The gravity the character falls at while floating")] public float FloatingGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while floating")] public float FloatingMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while floating")] public float FloatingMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (FloatingGravity)")] public float FloatingGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (FloatingGravity)")] public AnimationCurve FloatingGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when floating")] public float FloatingGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when floating")] public float FloatingGravityIncreaseDecelerationByFrame { get; private set; }


        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The gravity the character falls at while propulsing")] public float PropulsionGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while propulsing")] public float PropulsionMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while propulsing")] public float PropulsionMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (PropulsionGravity)")] public float PropulsionGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (PropulsionGravity)")] public AnimationCurve PropulsionGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when propulsing")] public float PropulsionGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when propulsing")] public float PropulsionGravityIncreaseDecelerationByFrame { get; private set; }


        [field: Header("Derive")]
        [field: SerializeField, Tooltip("The gravity the character falls at while deriving")] public float DeriveGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while deriving")] public float DeriveMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while deriving")] public float DeriveMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (DeriveGravity)")] public float DeriveGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (DeriveGravity)")] public AnimationCurve DeriveGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when deriving")] public float DeriveGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when deriving")] public float DeriveGravityIncreaseDecelerationByFrame { get; private set; }

        [field: Header("Ground Pound")]
        [field: SerializeField, Tooltip("The gravity the character falls at while ground pounding")] public float GroundPoundGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while ground pounding")] public float GroundPoundMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while ground pounding")] public float GroundPoundMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (GroundPoundGravity)")] public float GroundPoundGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (GroundPoundGravity)")] public AnimationCurve GroundPoundGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when ground pounding")] public float GroundPoundGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when ground pounding")] public float GroundPoundGravityIncreaseDecelerationByFrame { get; private set; }


    }
}
