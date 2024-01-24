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

        // Propulsion
        public PropulsionDirection PropulsionType { get; private set; }
        public bool StartedPropulsion { get; private set; }


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
            _inputs.Player.CameraMoveController.performed += SetCameraTypeController;

            _inputs.Player.UpPropulsion.performed += SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed += SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed += SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled += UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled += UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled += UnsetLateralPropulsion;
        }

        private void OnDisable()
        {
            _inputs.Disable();

            _inputs.Player.Move.performed -= OnMoveInput;
            _inputs.Player.Move.canceled -= OnMoveInput;

            _inputs.Player.CameraMoveMouse.performed -= SetCameraTypeMouse;
            _inputs.Player.CameraMoveController.performed -= SetCameraTypeController;

            _inputs.Player.UpPropulsion.performed -= SetUpPropulsion;
            _inputs.Player.DownPropulsion.performed -= SetDownPropulsion;
            _inputs.Player.LateralPropulsion.performed -= SetLateralPropulsion;
            _inputs.Player.UpPropulsion.canceled -= UnsetUpPropulsion;
            _inputs.Player.DownPropulsion.canceled -= UnsetDownPropulsion;
            _inputs.Player.LateralPropulsion.canceled -= UnsetLateralPropulsion;
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
            else LastMoveInputDirection = _propulsionDefaultDirection;
        }

        public void ResetLastMoveInputDirection()
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
            CameraMoveVector = context.ReadValue<Vector2>();
        }

        private void SetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= Inputs.PropulsionDirection.Up;
            StartedPropulsion = true;
        }

        private void UnsetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType &= ~Inputs.PropulsionDirection.Up;
            StartedPropulsion = true;
        }

        private void SetDownPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= Inputs.PropulsionDirection.Down;
            StartedPropulsion = true;
        }

        private void UnsetDownPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType &= ~Inputs.PropulsionDirection.Down;
        }

        private void SetLateralPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType |= Inputs.PropulsionDirection.Lateral;
        }

        private void UnsetLateralPropulsion(InputAction.CallbackContext context)
        {
            PropulsionType &= ~Inputs.PropulsionDirection.Lateral;
        }

        private void LateUpdate()
        {
            StartedPropulsion = false;
        }
    }
}