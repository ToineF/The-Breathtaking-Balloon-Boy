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
        
        [field: Header("Jump")]
        [field: SerializeField, Tooltip("The gravity the character falls at while jumping")] public float JumpGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while jumping")] public float JumpMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while jumping")] public float JumpMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (JumpGravity)")] public float JumpGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (JumpGravity)")] public AnimationCurve JumpGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when jumping")] public float JumpGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when jumping")] public float JumpGravityIncreaseDecelerationByFrame { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField, Tooltip("The gravity the character falls at while dashing")] public float DashGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while dashing")] public float DashMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while dashing")] public float DashMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (DashGravity)")] public float DashGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (DashGravity)")] public AnimationCurve DashGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame when dashing")] public float DashGravityIncreaseByFrame { get; private set; }
        [field: SerializeField, Tooltip("The deceleration of the increase of gravity added at each frame when dashing")] public float DashGravityIncreaseDecelerationByFrame { get; private set; }

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
