using UnityEngine;
using BlownAway.Character.Inputs;
using BlownAway.Camera;
using BlownAway.Character.Movements;
using BlownAway.Character.States;
using BlownAway.Character.Air;

namespace BlownAway.Character
{
    public class CharacterManager : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody CharacterRigidbody { get; set; }
        [field: SerializeField] public Collider CharacterCollider { get; set; }
        [field: SerializeField] public Transform CharacterVisual { get; set; }


        // Inputs
        [field: SerializeField, Tooltip("The reference to the class that contains the inputs of the character")] public CharacterInputsManager Inputs { get; private set; }

        // Camera
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the camera")] public CameraManager CameraManager { get; private set; }

        // Movement
        [field: SerializeField, Tooltip("The reference to the class that contains the movement of the character")] public CharacterMovementManager MovementManager { get; private set; }

        //States
        [field: SerializeField, Tooltip("The reference to the class that the states of the character")] public CharacterStatesManager States { get; private set; }

        // Air
        [field: SerializeField, Tooltip("The reference to the class that the logic of the air")] public CharacterAirManager AirManager { get; private set; }


    }
}