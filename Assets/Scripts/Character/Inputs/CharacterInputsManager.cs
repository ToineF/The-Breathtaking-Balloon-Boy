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
        public Vector3 MoveInputDirection { get; private set; }

        // Camera
        public bool IsMouse { get; private set; }
        public Vector2 CameraMoveVector { get; private set; }

        // Propulsion
        public PropulsionDirection PropulsionDirection { get; private set; }


        private PlayerInputs _inputs;

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

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            MoveInputDirection = new Vector3(xPosition, 0, zPosition);
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
            PropulsionDirection |= PropulsionDirection.Up;
        }

        private void UnsetUpPropulsion(InputAction.CallbackContext context)
        {
            PropulsionDirection &= ~PropulsionDirection.Up;
        }

        private void SetDownPropulsion(InputAction.CallbackContext context)
        {
            PropulsionDirection |= PropulsionDirection.Down;
        }

        private void UnsetDownPropulsion(InputAction.CallbackContext context)
        {
            PropulsionDirection &= ~PropulsionDirection.Down;
        }

        private void SetLateralPropulsion(InputAction.CallbackContext context)
        {
            PropulsionDirection |= PropulsionDirection.Lateral;
        }

        private void UnsetLateralPropulsion(InputAction.CallbackContext context)
        {
            PropulsionDirection &= ~PropulsionDirection.Lateral;
        }
    }
}