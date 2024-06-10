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

    public enum ControllerType
    {
        KEYBOARD = 0,
        XBOX = 1,
        PLAYSTATION = 2,
        SWITCH = 3,
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
        public bool JacketInflateToggle { get; private set; }
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

        // Menu
        public bool ConfirmUIPressed { get; private set; }
        public bool CancelUIPressed { get; private set; }
        public bool PausePressed { get; private set; }
        public bool SkipCutscene { get; private set; }

        // Inputs
        public Action<ControllerType> OnControllerTypeChange;
        public ControllerType ControllerType { get => _controllerType;
            set
            {
                if (_controllerType != value) OnControllerTypeChange?.Invoke(value);
                _controllerType = value;
            }
        }
        private PlayerInputs _inputs;
        private Gamepad _gamepad;
        private ControllerType _controllerType;
        public bool IsGamepad { get; private set; }

        // Propulsion
        private Vector3 _propulsionDefaultDirection = Vector3.forward;


        private void Awake()
        {
            _inputs = new PlayerInputs();

            if (Gamepad.all.Count > 0)
                _gamepad = Gamepad.all[0];
            AssignControllerType();
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

            _inputs.UnityUI.Submit.performed += UIConfirm;
            _inputs.UnityUI.Cancel.performed += UICancel;

            _inputs.Player.Pause.performed += StartPausePressed;

            _inputs.Player.SkipCutscene.performed += StartSkipCutscene;
            _inputs.Player.SkipCutscene.canceled += StopSkipCutscene;
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

            _inputs.UnityUI.Submit.performed -= UIConfirm;
            _inputs.UnityUI.Cancel.performed -= UICancel;

            _inputs.Player.Pause.performed -= StartPausePressed;

            _inputs.Player.SkipCutscene.performed -= StartSkipCutscene;
            _inputs.Player.SkipCutscene.canceled -= StopSkipCutscene;
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
            UpdateControllerType(context);
        }

        public void ResetLastPropulsionInputDirection()
        {
            PropulsionType = 0;
        }

        private void SetCameraTypeMouse(InputAction.CallbackContext context)
        {
            IsMouse = true;
            CameraMoveVector = context.ReadValue<Vector2>();
            UpdateControllerType(context);
        }

        private void SetCameraTypeController(InputAction.CallbackContext context)
        {
            IsMouse = false;
            CameraMoveVector = context.ReadValue<Vector2>() * Time.deltaTime;
            UpdateControllerType(context);
        }

        private void ResetCameraCenter(InputAction.CallbackContext context)
        {
            CameraCenter = true;
            UpdateControllerType(context);
        }

        private void StartCameraTopDown(InputAction.CallbackContext context)
        {
            CameraTopDownPressed = true;
            UpdateControllerType(context);
        }
        private void StopCameraTopDown(InputAction.CallbackContext context)
        {
            CameraTopDownReleased = true;
            UpdateControllerType(context);
        }

        private void SetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= PropulsionDirection.Up;
            StartPropulsion = true;
            UpdateControllerType(context);
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
            UpdateControllerType(context);
        }

        private void StopJumping(InputAction.CallbackContext context)
        {
            IsJumping = false;
            UpdateControllerType(context);
        }

        private void ToggleJacketInflation(InputAction.CallbackContext context)
        {
            JacketInflateToggle = true;
            UpdateControllerType(context);
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
            ConfirmUIPressed = false;
            CancelUIPressed = false;
            PausePressed = false;
            StartPropulsion = false;
            StartedJumping = false;

            UpdateControllerType();
        }

        private void UpdateControllerType()
        {
            if (!IsGamepad)
            {
                ControllerType = ControllerType.KEYBOARD;
            }
            else
            {
                if (_gamepad == null)
                {
                    ControllerType = ControllerType.KEYBOARD;
                    return;
                }

                AssignControllerType();
            }
        }

        private void AssignControllerType()
        {
            if (Gamepad.all.Count <= 0) return;
            var gamepad = Gamepad.all[0];

            if (gamepad is UnityEngine.InputSystem.XInput.XInputController)
            {
                ControllerType = ControllerType.XBOX;
            }
            else if (gamepad is UnityEngine.InputSystem.DualShock.DualShockGamepad)
            {
                ControllerType = ControllerType.PLAYSTATION;
            }
            else if (gamepad is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
            {
                ControllerType = ControllerType.SWITCH;
            }
            _gamepad = gamepad;
        }


        private void StartFalling(InputAction.CallbackContext context)
        {
            StartedFalling = true;
            UpdateControllerType(context);
        }

        private void StartGroundPound(InputAction.CallbackContext context)
        {
            StartedGroundPound = true;
            UpdateControllerType(context);
        }

        private void StartDash(InputAction.CallbackContext context)
        {
            StartedDash = true;
            UpdateControllerType(context);
        }

        private void UIConfirm(InputAction.CallbackContext context)
        {
            ConfirmUIPressed = true;
            UpdateControllerType(context);
        }

        private void UICancel(InputAction.CallbackContext context)
        {
            CancelUIPressed = true;
            UpdateControllerType(context);
        }

        private void StartSkipCutscene(InputAction.CallbackContext context)
        {
            SkipCutscene = true;
            UpdateControllerType(context);
        }

        private void StartPausePressed(InputAction.CallbackContext context)
        {
            PausePressed = true;
            UpdateControllerType(context);
        }

        private void StopSkipCutscene(InputAction.CallbackContext context)
        {
            SkipCutscene = false;
            UpdateControllerType(context);
        }

        private void UpdateControllerType(InputAction.CallbackContext context)
        {
            IsGamepad = (context.action.activeControl.device.name != "Keyboard" && context.action.activeControl.device.name != "Mouse");
        }
    }
}