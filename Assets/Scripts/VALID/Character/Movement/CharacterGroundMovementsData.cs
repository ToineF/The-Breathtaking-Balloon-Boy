using UnityEngine;


namespace BlownAway.Character.Movements.Data
{
    [CreateAssetMenu(fileName = "CharacterGroundMovementsData", menuName = "MovementsData/Ground")]
    public class CharacterGroundMovementsData : ScriptableObject
    {
        [field: SerializeField, Tooltip("The maximum walking speed the character aims to moves at")] public float BaseWalkSpeed { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public float BaseWalkTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to reach the maximum speed (BaseWalkSpeed) after pressing the inputs")] public AnimationCurve BaseWalkCurve { get; set; }
        [field: SerializeField, Tooltip("The time it takes to the character to stop moving after releasing the inputs")] public float BaseIdleTime { get; set; }
        [field: SerializeField, Tooltip("The lerp used by character to stop moving after releasing the inputs")] public AnimationCurve BaseIdleCurve { get; set; }
        [field: SerializeField, Range(0, 1), Tooltip("The time it takes to the character to transition from its current direction to the target direction")] public float WalkDirectionTransitionSpeed { get; set; }
    }
}