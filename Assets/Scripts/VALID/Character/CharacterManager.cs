using UnityEngine;
using BlownAway.Character.Inputs;
using BlownAway.Camera;
using BlownAway.Character.Movements;
using BlownAway.Character.States;
using BlownAway.Character.Air;
using BlownAway.Character.Animations;
using BlownAway.Hitbox.Checkpoints;
using BlownAway.Character.Data;

namespace BlownAway.Character
{
    [DefaultExecutionOrder(-1)]
    public class CharacterManager : MonoBehaviour
    {
        [field: Header("References to data")]
        [field: SerializeField] public Transform CharacterVisual { get; set; }

        // Data
        [Tooltip("The current data of the character")] public CharacterControllerData Data { get; set; }


        [field:Header("References to scripts")]
        // Inputs
        [field: SerializeField, Tooltip("The reference to the class that contains the inputs of the character")] public CharacterInputsManager Inputs { get; private set; }

        // Camera
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the camera")] public CharacterCameraManager CameraManager { get; private set; }

        // Movement
        [field: SerializeField, Tooltip("The reference to the class that contains the movement of the character")] public CharacterMovementManager MovementManager { get; private set; }

        // States
        [field: SerializeField, Tooltip("The reference to the class that contains the states of the character")] public CharacterStatesManager States { get; private set; }
        
        // Collider
        [field: SerializeField, Tooltip("The reference to the class that contains the collider the character")] public CharacterCollider CharacterCollider { get; private set; }

        // Air
        [field: SerializeField, Tooltip("The reference to the class that containsthe logic of the air")] public CharacterAirManager AirManager { get; private set; }

        // Animations
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the animations")] public CharacterAnimationManager AnimationManager { get; private set; }

        // Checkpoints
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the checkpoint system")] public CheckpointManager CheckpointManager { get; private set; }

        // Children
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the checkpoint system")] public CharacterChildrenManager ChildrenManager { get; private set; }

        // Feedbacks
        [field: SerializeField, Tooltip("The reference to the class that contains the references to the feedbacks")] public CharacterFeedbacksManager Feedbacks { get; private set; }

        // UI Transition
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the transitions")] public Transition Transition { get; private set; }

        // Debug Mode
        [field: SerializeField, Tooltip("The reference to the class that contains the logic of the transitions")] public DebugMode.DebugMode DebugMode { get; private set; }



        private void Awake()
        {
            DebugMode.InitScript(this);
            Inputs.InitScript(this);
            ChildrenManager.InitScript(this);
            CameraManager.InitScript(this);
            MovementManager.InitScript(this);
            States.InitScript(this);
            CharacterCollider.InitScript(this);
            AirManager.InitScript(this);
            AnimationManager.InitScript(this);
            CharacterCollider.InitScript(this);
        }


    }
}