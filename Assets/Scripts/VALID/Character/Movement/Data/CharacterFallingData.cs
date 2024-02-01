using UnityEngine;

[CreateAssetMenu(fileName = "FallingData", menuName = "MovementsData/Fall")]
public class CharacterFallingData : ScriptableObject
{
    [field:Header("Base")]
    [field:SerializeField, Tooltip("The gravity the character falls at while not floating")] public float BaseGravity {get; set;}
    [field: SerializeField, Tooltip("The minimum gravity the character can fall at while not floating")] public float BaseMinGravity {get; set;}
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while not floating")] public float BaseMaxGravity {get; set;}
    [field: SerializeField, Tooltip("The time needed to reach the target gravity (BaseGravity)")] public float BaseGravityTime {get; set;}
    [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (BaseGravity)")] public AnimationCurve BaseGravityAccel {get; set;}
    [field: SerializeField, Tooltip("The increase of gravity added at each frame")] public float GravityIncreaseByFrame {get; set;}

    [field: Header("Floating")]
    [field: SerializeField, Tooltip("The gravity the character falls at while floating")] public float FloatingGravity {get; set;}
    [field: SerializeField, Tooltip("The minimum gravity the character can fall at while floating")] public float FloatingMinGravity { get; set; }
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while floating")] public float FloatingMaxGravity { get; set; }
    [field: SerializeField, Tooltip("The time needed to reach the target gravity (FloatingGravity)")] public float FloatingGravityTime { get; set; }
    [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (FloatingGravity)")] public AnimationCurve FloatingGravityAccel { get; set; }

    [field: Header("Propulsion")]
    [field: SerializeField, Tooltip("The gravity the character falls at while propulsing")] public float PropulsionGravity {get; set;}
    [field: SerializeField, Tooltip("The minimum gravity the character can fall at while propulsing")] public float PropulsionMinGravity {get; set;}
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while propulsing")] public float PropulsionMaxGravity {get; set;}
    [field: SerializeField, Tooltip("The time needed to reach the target gravity (PropulsionGravity)")] public float PropulsionGravityTime { get; set; }
    [field: SerializeField, Tooltip("The lerp used by the character to reach the target gravity (PropulsionGravity)")] public AnimationCurve PropulsionGravityAccel { get; set; }

}
