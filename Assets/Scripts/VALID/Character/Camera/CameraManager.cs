using System;
using UnityEngine;
using BlownAway.Character;

namespace BlownAway.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraEntity _cameraParams;
        [SerializeField] private bool _setCursorVisible;

        private Vector2 _currentCameraAngle;

        // test
        [SerializeField] private float hitOffset;
        [SerializeField] private float _cameraCollisionRadius;
        [SerializeField] private LayerMask solidsMask;

        private void Start()
        {
            Cursor.lockState = _setCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _setCursorVisible;
        }

        public void UpdateCameraPosition() // Update
        {
            float YPosition = _cameraParams.YOffset + _currentCameraAngle.y;
            Vector3 cameraVector = new Vector3((float)Math.Cos(_currentCameraAngle.x), YPosition, (float)Math.Sin(_currentCameraAngle.x)).normalized * int.MaxValue;
            Vector3 newPosition = _cameraParams.transform.position + cameraVector;
            if (!Physics.SphereCast(_cameraParams.transform.position, _cameraCollisionRadius, cameraVector.normalized, out RaycastHit hit, cameraVector.magnitude, solidsMask, QueryTriggerInteraction.Ignore))
            {
              _cameraParams.transform.position = newPosition;
              _cameraParams.FramingTransposer.m_TrackedObjectOffset = _cameraParams.PositionOffset;
              _cameraParams.FramingTransposer.m_CameraDistance = _cameraParams.CameraDistance;
            } else
            {
              _cameraParams.transform.position = hit.point + (_cameraParams.transform.position - newPosition).normalized * hitOffset;
            }

            //if (!Physics.Linecast(_cameraParams.transform.position, newPosition, out RaycastHit hit, solidsMask, QueryTriggerInteraction.Ignore))

            //

            //Vector3 direction = GameManager.Instance.CharacterManager.CharacterTransform.position - _cameraParams.transform.position;
            // Debug.Log (Physics.SphereCast(_cameraParams.transform.position, _cameraCollisionRadius, direction, out RaycastHit hit, int.MaxValue, solidsMask, QueryTriggerInteraction.Ignore)) ;
            // if (Physics.Linecast(_cameraParams.transform.position, newPosition, out RaycastHit hit, solidsMask, QueryTriggerInteraction.Ignore))
            //_cameraParams.transform.position = hit.point + (_cameraParams.transform.position - newPosition).normalized * hitOffset;
            //else
            //    _cameraParams.transform.position = newPosition;
            //



        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            if (Time.timeScale == 0) return;

            float sensitivity = manager.Inputs.IsMouse ? _cameraParams.MouseSensitivity : _cameraParams.ControllerSensitivity;
            float xSign = (manager.Inputs.IsMouse ? _cameraParams.IsMouseXInverted : _cameraParams.IsControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? _cameraParams.IsMouseYInverted : _cameraParams.IsControllerXInverted) ? -1 : 1;
            _currentCameraAngle += new Vector2(manager.Inputs.CameraMoveVector.x * xSign, manager.Inputs.CameraMoveVector.y * ySign % 360) * sensitivity;
            _currentCameraAngle.y = Math.Clamp(_currentCameraAngle.y, -_cameraParams.YDeadZone, _cameraParams.YDeadZone);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_cameraParams.transform.position, GameManager.Instance.CharacterManager.CharacterTransform.position - _cameraParams.transform.position);
        }
#endif
    }
}