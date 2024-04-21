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

        private CharacterCameraData _cameraData;

        private void Awake()
        {
            _isXInverted.onValueChanged.AddListener(UpdateCameraX);
            _isYInverted.onValueChanged.AddListener(UpdateCameraY);
        }

        protected override void StartScript(CharacterManager manager)
        {
            base.StartScript(manager);

            // Camera
            _cameraData = Manager.Data.CameraData;
            _isXInverted.isOn = _cameraData.IsXInvertedDefault;
            _isYInverted.isOn = _cameraData.IsYInvertedDefault;
            UpdateCameraX(_cameraData.IsXInvertedDefault);
            UpdateCameraY(_cameraData.IsYInvertedDefault);
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
    }
}