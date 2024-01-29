using System;
using UnityEngine;
using BlownAway.Character;

namespace BlownAway.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraEntity CameraParams;
        private Vector2 _currentCameraAngle;


        public void UpdateCameraPosition() // Update
        {
            float YPosition = CameraParams.YOffset + _currentCameraAngle.y;
            Vector3 cameraVector = new Vector3((float)Math.Cos(_currentCameraAngle.x), YPosition, (float)Math.Sin(_currentCameraAngle.x)).normalized * int.MaxValue;
            Vector3 newPosition = transform.position + cameraVector;
            CameraParams.transform.position = newPosition;

            CameraParams.FramingTransposer.m_TrackedObjectOffset = CameraParams.PositionOffset;
            CameraParams.FramingTransposer.m_CameraDistance = CameraParams.CameraDistance;
        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            if (Time.timeScale == 0) return;

            float sensitivity = manager.Inputs.IsMouse ? CameraParams.MouseSensitivity : CameraParams.ControllerSensitivity;
            float xSign = (manager.Inputs.IsMouse ? CameraParams.IsMouseXInverted : CameraParams.IsControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? CameraParams.IsMouseYInverted : CameraParams.IsControllerXInverted) ? -1 : 1;
            _currentCameraAngle += new Vector2(manager.Inputs.CameraMoveVector.x * xSign, manager.Inputs.CameraMoveVector.y * ySign) * sensitivity;
            _currentCameraAngle.y = Math.Clamp(_currentCameraAngle.y, -CameraParams.YDeadZone, CameraParams.YDeadZone);
        }
    }
}