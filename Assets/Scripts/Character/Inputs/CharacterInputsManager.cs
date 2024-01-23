using UnityEngine;
using UnityEngine.InputSystem;

namespace BlownAway.Character.Inputs
{
    public class CharacterInputsManager : MonoBehaviour
    {
        public Vector3 MoveInputDirection { get; private set; }
        public bool IsMouse { get; private set; }
        public Vector2 CameraMoveVector { get; private set; }

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
        }

        private void OnDisable()
        {
            _inputs.Disable();
            _inputs.Player.Move.performed -= OnMoveInput;
            _inputs.Player.Move.canceled -= OnMoveInput;
            _inputs.Player.CameraMoveMouse.performed -= SetCameraTypeMouse;
            _inputs.Player.CameraMoveController.performed -= SetCameraTypeController;
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            MoveInputDirection = new Vector3(xPosition, 0, zPosition);
        }
        public void SetCameraTypeMouse(InputAction.CallbackContext context)
        {
            IsMouse = true;
            CameraMoveVector = context.ReadValue<Vector2>();
        }

        public void SetCameraTypeController(InputAction.CallbackContext context)
        {
            IsMouse = false;
            CameraMoveVector = context.ReadValue<Vector2>();
        }
    }
}