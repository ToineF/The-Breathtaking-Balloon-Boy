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

    public class CharacterInputsManager : MonoBehaviour
    {
        // Movements
        [Tooltip("The input direction for the movement")] public Vector3 MoveInputDirection { get; private set; }
        [Tooltip("The last non-null input direction for the movement")] public Vector3 LastMoveInputDirection { get; private set; }

        // Camera
        public bool IsMouse { get; private set; }
        public Vector2 CameraMoveVector { get; private set; }
        public bool CameraCenter { get; private set; }

        // Propulsion
        public PropulsionDirection PropulsionType { get; private set; }
        public bool IsJacketInflated { get => PropulsionType != 0; }

        // Falling
        public bool StartedFalling { get; private set; }

        // Upgrades
        public bool StartedBalloonBounce { get; private set; }
        public bool StartedGroundPound { get; private set; }


        // Inputs
        private PlayerInputs _inputs;

        // Propulsion
        private static Vector3 _propulsionDefaultDirection = Vector3.forward;


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

            _inputs.Player.UpPropulsion.performed += SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed += SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed += SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled += UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled += UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled += UnsetLateralPropulsion;

            _inputs.Player.CancelPropulsion.performed += StartFalling;

            _inputs.Player.BalloonBounce.performed += StartBalloonBounce;
            _inputs.Player.GroundPound.performed += StartGroundPound;
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

            _inputs.Player.UpPropulsion.performed -= SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed -= SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed -= SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled -= UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled -= UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled -= UnsetLateralPropulsion;

            _inputs.Player.CancelPropulsion.performed -= StartFalling;

            _inputs.Player.BalloonBounce.performed -= StartBalloonBounce;
            _inputs.Player.GroundPound.performed -= StartGroundPound;
        }

        private void Start()
        {
            LastMoveInputDirection = _propulsionDefaultDirection;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            MoveInputDirection = new Vector3(xPosition, 0, zPosition);
            if (MoveInputDirection != Vector3.zero) LastMoveInputDirection = MoveInputDirection;
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

        private void SetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= PropulsionDirection.Up;
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

        private void LateUpdate()
        {
            StartedFalling = false;
            StartedBalloonBounce = false;
            StartedGroundPound = false;
            CameraCenter = false;
        }

        private void StartFalling(InputAction.CallbackContext context)
        {
            StartedFalling = true;
        }

        private void StartBalloonBounce(InputAction.CallbackContext context)
        {
            StartedBalloonBounce = true;
        }

        private void StartGroundPound(InputAction.CallbackContext context)
        {
            StartedGroundPound = true;
        }
    }
}