using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "FallingData", menuName = "MovementsData/Fall")]
    public class CharacterFallingData : ScriptableObject
    {
        [field: Header("Base")]
        [field: SerializeField, Tooltip("The gravity the character falls at while not floating")] public float BaseGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while not floating")] public float BaseMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while not floating")] public float BaseMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (BaseGravity)")] public float BaseGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (BaseGravity)")] public AnimationCurve BaseGravityAccel { get; private set; }
        [field: SerializeField, Tooltip("The increase of gravity added at each frame")] public float GravityIncreaseByFrame { get; private set; }

        [field: Header("Floating")]
        [field: SerializeField, Tooltip("The gravity the character falls at while floating")] public float FloatingGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while floating")] public float FloatingMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while floating")] public float FloatingMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (FloatingGravity)")] public float FloatingGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (FloatingGravity)")] public AnimationCurve FloatingGravityAccel { get; private set; }

        [field: Header("Propulsion")]
        [field: SerializeField, Tooltip("The gravity the character falls at while propulsing")] public float PropulsionGravity { get; private set; }
        [field: SerializeField, Tooltip("The minimum gravity the character can fall at while propulsing")] public float PropulsionMinGravity { get; private set; }
        [field: SerializeField, Tooltip("The maximum gravity the character can fall at while propulsing")] public float PropulsionMaxGravity { get; private set; }
        [field: SerializeField, Tooltip("The time needed to reach the target gravity (PropulsionGravity)")] public float PropulsionGravityTime { get; private set; }
        [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (PropulsionGravity)")] public AnimationCurve PropulsionGravityAccel { get; private set; }

    }
}
