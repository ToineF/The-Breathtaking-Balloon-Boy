using UnityEngine;
using AntoineFoucault.Utilities;
using BlownAway.Character;
using BlownAway.Camera.Data;

namespace BlownAway.Camera
{
    public class CharacterCameraManager : MonoBehaviour
    {
        public CharacterManager Manager { get; set; }

        [SerializeField] private CharacterCameraData _cameraData;

        [Header("References")]
        [SerializeField] GameObject FocusPoint;
        [SerializeField] GameObject CameraCenter;
        [SerializeField] UnityEngine.Camera Camera;

        private Vector3 _cameraMoveVector;

        private Vector3 _camDist;
        private float _zoomDistance;
        private float _sensitivity;

        private RaycastHit _camHit;
        private RaycastHit _camHit2;

        private void Start()
        {
            _camDist = Camera.transform.localPosition;
            _zoomDistance = _cameraData.ZoomDefault;
            _camDist.z = -_zoomDistance;

            Cursor.lockState = _cameraData.SetCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _cameraData.SetCursorVisible;
        }

        private void LateUpdate()
        {
            UpdateCameraAngle(Manager);
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            SetCameraAngle();

            ScrollCamera();

            SetCameraPosition();
        }

        private void SetCameraPosition()
        {
            if (_camDist.z != -_zoomDistance)
            {
                _camDist.z = Mathf.Lerp(_camDist.z, -_zoomDistance, Time.deltaTime * _cameraData.ScrollDampening);
            }

            Camera.transform.localPosition = _camDist;

            GameObject obj = new GameObject();
            obj.transform.SetParent(Camera.transform.parent);
            obj.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                Camera.transform.localPosition.z - _cameraData.CollisionSensitivity);

            Vector3 direction = obj.transform.position - Camera.transform.position;
            direction.Normalize();
            Debug.DrawLine(Camera.transform.position, CameraCenter.transform.position + direction * _cameraData.CollisionSensitivity, Color.red);

            if (Physics.Linecast(CameraCenter.transform.position + direction * _cameraData.CollisionSensitivity, Camera.transform.position, out _camHit, ~_cameraData.PlayerLayer, QueryTriggerInteraction.Ignore))
            {
                Physics.Linecast(Camera.transform.position, CameraCenter.transform.position + direction * _cameraData.CollisionSensitivity, out _camHit2, ~_cameraData.PlayerLayer, QueryTriggerInteraction.Ignore);
                //Debug.Log("Distance : " + Vector3.Distance(_camHit.point, _camHit2.point));

                if (Vector3.Distance(_camHit.point, _camHit2.point) > _cameraData.MinCollisionThickness)
                {

                    Camera.transform.position = _camHit.point;

                    var localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                        Camera.transform.localPosition.z + _cameraData.CollisionSensitivity);

                    Camera.transform.localPosition = localPosition;
                }

            }

            Destroy(obj);

            if (Camera.transform.localPosition.z > -1f)
            {
                Camera.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y, -1f);
            }
        }

        private void SetCameraAngle()
        {
            CameraCenter.transform.position = new Vector3(FocusPoint.transform.position.x,
                                        FocusPoint.transform.position.y + _cameraData.YOffset, FocusPoint.transform.position.z);

            float xAngle = CameraCenter.transform.rotation.eulerAngles.x - _cameraMoveVector.y * _sensitivity / 2;
            float yAngle = CameraCenter.transform.rotation.eulerAngles.y + _cameraMoveVector.x * _sensitivity;

            var rotation = Quaternion.Euler(
                MathExtentions.ClampAngle(xAngle, _cameraData.YDownLimit, _cameraData.YUpLimit),
                yAngle,
                CameraCenter.transform.rotation.eulerAngles.z);

            CameraCenter.transform.rotation = rotation;
        }

        private void ScrollCamera()
        {
            if (!_cameraData.CanScroll) return;

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * _cameraData.ScrollSensitivity;
                scrollAmount *= _zoomDistance * 0.3f;
                _zoomDistance -= scrollAmount;
                _zoomDistance = Mathf.Clamp(_zoomDistance, _cameraData.ZoomMin, _cameraData.ZoomMax);
            }
        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            _sensitivity = manager.Inputs.IsMouse ? _cameraData.MouseSensitivity : _cameraData.ControllerSensitivity;
            float xSign = (manager.Inputs.IsMouse ? _cameraData.IsMouseXInverted : _cameraData.IsControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? _cameraData.IsMouseYInverted : _cameraData.IsControllerYInverted) ? -1 : 1;
            _cameraMoveVector = manager.Inputs.CameraMoveVector;
            _cameraMoveVector = new Vector3(_cameraMoveVector.x * xSign, _cameraMoveVector.y * ySign);
        }
    }
}