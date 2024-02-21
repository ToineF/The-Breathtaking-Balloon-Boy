using UnityEngine;
using BlownAway.Character.Air.Data;
using BlownAway.Character.Movements.Data;
using BlownAway.Camera.Data;

namespace BlownAway.Character.Data 
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/Global Data")]
    public class CharacterControllerData : ScriptableObject
    {
        // Idle & Walk Data
        [field: SerializeField] public CharacterLateralMovementsData LateralMovementData { get; private set; }

        // Fall Data
        [field: SerializeField] public CharacterFallingData FallData { get; private set; }

        // Propulsion Data
        [field: SerializeField] public CharacterPropulsionData PropulsionData { get; private set; }

        // Ground Detection Data
        [field: SerializeField] public CharacterGroundDetectionData GroundDetectionData { get; private set; }

        // Slopes Data
        [field: SerializeField] public CharacterSlopesData SlopeData { get; private set; }

        // Air Data
        [field: SerializeField] public CharacterAirData AirData { get; private set; }

        // Camera Data
        [field:SerializeField] public CharacterCameraData CameraData { get; private set; }

    }
}