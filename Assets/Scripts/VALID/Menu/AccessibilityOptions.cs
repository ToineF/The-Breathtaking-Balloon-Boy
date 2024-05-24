using BlownAway.Camera.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BlownAway.Character
{
    public class AccessibilityOptions : CharacterSubComponent
    {
        [Header("Camera")]
        [SerializeField] private Toggle _isXInverted;
        [SerializeField] private Toggle _isYInverted;
        [SerializeField] private Slider _controllerSensitivity;
        [SerializeField] private Slider _mouseSensitivity;

        private CharacterCameraData _cameraData;

        private void Awake()
        {
            _isXInverted.onValueChanged.AddListener(UpdateCameraX);
            _isYInverted.onValueChanged.AddListener(UpdateCameraY);
            _controllerSensitivity.onValueChanged.AddListener(UpdateControllerSensitivity);
            _mouseSensitivity.onValueChanged.AddListener(UpdateMouseSensitivity);
        }

        protected override void StartScript(CharacterManager manager)
        {
            base.StartScript(manager);

            // Camera inversion
            _cameraData = Manager.Data.CameraData;
            _isXInverted.isOn = _cameraData.IsXInverted;
            _isYInverted.isOn = _cameraData.IsYInverted;
            UpdateCameraX(_cameraData.IsXInverted);
            UpdateCameraY(_cameraData.IsYInverted);

            // Camera sensitivity
            _controllerSensitivity.value = _cameraData.ControllerSensitivity;
            _mouseSensitivity.value = _cameraData.MouseSensitivity;
            UpdateControllerSensitivity(_cameraData.ControllerSensitivity);
            UpdateMouseSensitivity(_cameraData.MouseSensitivity);
        }

        private void UpdateCameraX(bool isInverted)
        {
            _cameraData.IsMouseXInverted = isInverted;
            _cameraData.IsControllerXInverted = isInverted;
        }

        private void UpdateCameraY(bool isInverted)
        {
            _cameraData.IsMouseYInverted = isInverted;
            _cameraData.IsControllerYInverted = isInverted;
        }

        private void UpdateControllerSensitivity(float value)
        {
            _cameraData.ControllerSensitivityGameplay = value;
        }

        private void UpdateMouseSensitivity(float value)
        {
            _cameraData.MouseSensitivityGameplay = value;
        }
    }
}