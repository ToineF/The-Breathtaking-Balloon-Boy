using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BlownAway.Character.Inputs
{

    [Flags]
    public enum PropulsionDirection
    {
        Up = 1,
        Down = 2,
        Lateral = 4,
    }

    public class CharacterInputsManager : CharacterSubComponent
    {
        // Movements
        [Tooltip("The input direction for the movement")] public Vector3 MoveInputDirection { get; private set; }
        [Tooltip("The last non-null input direction for the movement")] public Vector3 LastMoveInputDirection { get; private set; }
        public bool StartMoving { get; private set; }

        // Camera
        public bool IsMouse { get; private set; }
        public Vector2 CameraMoveVector { get; private set; }
        public bool CameraCenter { get; private set; }
        public bool CameraTopDownPressed { get; private set; }
        public bool CameraTopDownReleased { get; private set; }

        // Propulsion
        public bool JacketInflateToggle {get; private set;}
        public PropulsionDirection PropulsionType { get; private set; }
        public bool StartPropulsion { get; private set; }

        // Jump
        public bool StartedJumping { get; private set; }
        public bool IsJumping { get; private set; }

        // Falling
        public bool StartedFalling { get; private set; }

        // Upgrades
        public bool StartedDash { get; private set; }
        public bool StartedGroundPound { get; private set; }

        // Cutscenes
        public bool NextDialoguePressed { get; private set; }
    

        // Inputs
        private PlayerInputs _inputs;

        // Propulsion
        private Vector3 _propulsionDefaultDirection = Vector3.forward;


        private void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            _inputs.Enable();

            _inputs.Player.Move.performed += OnMoveInput;
            _inputs.Player.Move.canceled += OnMoveInput;

            _inputs.Player.CameraMoveMouse.performed += SetCameraTypeMouse;
            _inputs.Player.CameraMoveMouse.canceled += SetCameraTypeMouse;
            _inputs.Player.CameraMoveController.performed += SetCameraTypeController;
            _inputs.Player.CameraMoveController.canceled += SetCameraTypeController;

            _inputs.Player.CameraCenter.performed += ResetCameraCenter;
            _inputs.Player.CameraTopDown.performed += StartCameraTopDown;
            _inputs.Player.CameraTopDown.canceled += StopCameraTopDown;

            _inputs.Player.UpPropulsion.performed += SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed += SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed += SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled += UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled += UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled += UnsetLateralPropulsion;

            _inputs.Player.Jump.performed += StartJumping;
            _inputs.Player.Jump.canceled += StopJumping;

            _inputs.Player.InflateJacket.performed += ToggleJacketInflation;

            _inputs.Player.CancelPropulsion.performed += StartFalling;

            _inputs.Player.Dash.performed += StartDash;
            _inputs.Player.GroundPound.performed += StartGroundPound;

            _inputs.Player.NextDialogue.performed += PlayNextDialogue;
        }

        private void OnDisable()
        {
            _inputs.Disable();

            _inputs.Player.Move.performed -= OnMoveInput;
            _inputs.Player.Move.canceled -= OnMoveInput;

            _inputs.Player.CameraMoveMouse.performed -= SetCameraTypeMouse;
            _inputs.Player.CameraMoveMouse.canceled -= SetCameraTypeMouse;
            _inputs.Player.CameraMoveController.performed -= SetCameraTypeController;
            _inputs.Player.CameraMoveController.canceled -= SetCameraTypeController;

            _inputs.Player.CameraCenter.performed -= ResetCameraCenter;
            _inputs.Player.CameraTopDown.performed -= StartCameraTopDown;
            _inputs.Player.CameraTopDown.canceled -= StopCameraTopDown;

            _inputs.Player.UpPropulsion.performed -= SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed -= SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed -= SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled -= UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled -= UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled -= UnsetLateralPropulsion;

            _inputs.Player.InflateJacket.performed -= ToggleJacketInflation;

            _inputs.Player.CancelPropulsion.performed -= StartFalling;

            _inputs.Player.Dash.performed -= StartDash;
            _inputs.Player.GroundPound.performed -= StartGroundPound;

            _inputs.Player.NextDialogue.performed -= PlayNextDialogue;
        }

        public void EnableInputs(bool enabled)
        {
            if (enabled) OnEnable();
            else OnDisable();
        }

        private void Start()
        {
            LastMoveInputDirection = _propulsionDefaultDirection;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            Vector3 previousPosition = MoveInputDirection;
            MoveInputDirection = new Vector3(xPosition, 0, zPosition);
            if (MoveInputDirection != Vector3.zero) LastMoveInputDirection = MoveInputDirection;
            if (previousPosition == Vector3.zero && MoveInputDirection != Vector3.zero) StartMoving = true;
            //else LastMoveInputDirection = _propulsionDefaultDirection; // Uncomment if you want transform.forward to be the default position
        }

        public void ResetLastPropulsionInputDirection()
        {
            PropulsionType = 0;
        }

        private void SetCameraTypeMouse(InputAction.CallbackContext context)
        {
            IsMouse = true;
            CameraMoveVector = context.ReadValue<Vector2>();
        }

        private void SetCameraTypeController(InputAction.CallbackContext context)
        {
            IsMouse = false;
            CameraMoveVector = context.ReadValue<Vector2>() * Time.deltaTime;
        }

        private void ResetCameraCenter(InputAction.CallbackContext context)
        {
            CameraCenter = true;
        }

        private void StartCameraTopDown(InputAction.CallbackContext context)
        {
            CameraTopDownPressed = true;
        }
        private void StopCameraTopDown(InputAction.CallbackContext context)
        {
            CameraTopDownReleased = true;
        }

        private void SetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= PropulsionDirection.Up;
            StartPropulsion = true;
        }

        private void UnsetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType &= ~PropulsionDirection.Up;
        }

        private void SetDownPropulsion(InputAction.CallbackContext context)
        {
            //PropulsionType |= PropulsionDirection.Down;
        }

        private void UnsetDownPropulsion(InputAction.CallbackContext context)
        {
            //PropulsionType &= ~PropulsionDirection.Down;
        }

        private void SetLateralPropulsion(InputAction.CallbackContext context)
        {
            //PropulsionType |= PropulsionDirection.Lateral;
        }

        private void UnsetLateralPropulsion(InputAction.CallbackContext context)
        {
            //PropulsionType &= ~PropulsionDirection.Lateral;
        }

        private void StartJumping(InputAction.CallbackContext context)
        {
            IsJumping = true;
            StartedJumping = true;
        }

        private void StopJumping(InputAction.CallbackContext context)
        {
            IsJumping = false;
        }

        private void ToggleJacketInflation(InputAction.CallbackContext context)
        {
            JacketInflateToggle = true;
        }

        private void LateUpdate()
        {
            JacketInflateToggle = false;
            StartedFalling = false;
            StartedDash = false;
            StartedGroundPound = false;
            StartMoving = false;
            CameraCenter = false;
            CameraTopDownPressed = false;
            CameraTopDownReleased = false;
            NextDialoguePressed = false;
            StartPropulsion = false;
            StartedJumping = false;
        }

        private void StartFalling(InputAction.CallbackContext context)
        {
            StartedFalling = true;
        }

        private void StartGroundPound(InputAction.CallbackContext context)
        {
            StartedGroundPound = true;
        }

        private void StartDash(InputAction.CallbackContext context)
        {
            StartedDash = true;
        }

        private void PlayNextDialogue(InputAction.CallbackContext context)
        {
            NextDialoguePressed = true;
        }
    }
}