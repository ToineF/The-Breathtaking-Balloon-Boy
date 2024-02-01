using UnityEngine;

[CreateAssetMenu(fileName = "FallingData", menuName = "MovementsData/Fall")]
public class CharacterFallingData : ScriptableObject
{
    [Tooltip("The current gravity the character falls at")] public float CurrentGravity { get; set; }
    [Tooltip("The minimum gravity the character can fall at")] public float MinGravity {get; set;}
    [Tooltip("The maximum gravity the character can fall at")] public float MaxGravity {get; set;}
    [field:SerializeField, Tooltip("The gravity the character falls at while not floating")] public float BaseGravity {get; set;}
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while not floating")] public float BaseMaxGravity {get; set;}
    [field: SerializeField, Tooltip("The increase of gravity added at each frame")] public float GravityIncreaseByFrame {get; set;}
    [field: SerializeField, Tooltip("The gravity the character falls at while floating")] public float FloatingGravity {get; set;}
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while floating")] public float FloatingMaxGravity;
    [field: SerializeField, Tooltip("The gravity the character falls at while propulsing")] public float PropulsionGravity {get; set;}
    [field: SerializeField, Tooltip("The maximum gravity the character can fall at while propulsing")] public float PropulsionMaxGravity {get; set;}
    
}
