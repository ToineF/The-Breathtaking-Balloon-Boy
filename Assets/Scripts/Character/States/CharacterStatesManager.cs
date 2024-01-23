using UnityEngine;
using UnityEngine.Windows;

namespace BlownAway.Character.States
{

    public class CharacterStatesManager : MonoBehaviour
    {

        // States Bank
        public CharacterIdleState IdleState = new CharacterIdleState();
        public CharacterWalkingState WalkingState = new CharacterWalkingState();
        public CharacterFallingState FallingState = new CharacterFallingState();
        public CharacterFloatingState FloatingState = new CharacterFloatingState();
        public CharacterPropulsionState PropulsionState = new CharacterPropulsionState();
        public CharacterDeathState DeathState = new CharacterDeathState();
        public CharacterCutsceneState CutsceneState = new CharacterCutsceneState();

        // Inputs
        public PlayerInputs InputActions { get; private set; }


        private CharacterBaseState _currentState;


        /// MOVE INPUTS IN ANOTHER SCRIPT
        #region Inputs 
        private void Awake()
        {
            InputActions = new PlayerInputs();
        }
        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
            InputActions.Player.CameraMoveMouse.performed -= CharacterManager.Instance.SetCameraTypeMouse;
            InputActions.Player.CameraMoveMouse.canceled -= CharacterManager.Instance.SetCameraTypeMouse;
            InputActions.Player.CameraMoveController.performed -= CharacterManager.Instance.SetCameraTypeController;
            InputActions.Player.CameraMoveController.canceled -= CharacterManager.Instance.SetCameraTypeController;
        }
        #endregion

        private void Start()
        {
            // Inputs (move in another state)
            InputActions.Player.CameraMoveMouse.performed += CharacterManager.Instance.SetCameraTypeMouse;
            InputActions.Player.CameraMoveMouse.canceled += CharacterManager.Instance.SetCameraTypeMouse;
            InputActions.Player.CameraMoveController.performed += CharacterManager.Instance.SetCameraTypeController;
            InputActions.Player.CameraMoveController.canceled += CharacterManager.Instance.SetCameraTypeController;

            _currentState = FallingState;
            _currentState.EnterState(this);
           
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        private void FixedUpdate()
        {
            CharacterManager.Instance.ResetVelocity();
            _currentState.FixedUpdateState(this);
            CharacterManager.Instance.ApplyVelocity();
        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(this);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(this);

            _currentState = state;

            _currentState.EnterState(this);
        }
    }
}
