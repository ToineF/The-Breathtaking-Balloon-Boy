using UnityEngine;
using AntoineFoucault.Utilities;
using BlownAway.Character;
using BlownAway.Camera.Data;

namespace BlownAway.Camera
{
    public class CharacterCameraManager : MonoBehaviour
    {
        public CharacterManager Manager { get; set; }


        [field:Header("References")]
        [field:SerializeField] public UnityEngine.Camera Camera { get; private set; }
        [SerializeField] GameObject FocusPoint;
        [SerializeField] GameObject CameraCenter;

        private Vector3 _cameraMoveVector;

        private Vector3 _camDist;
        private float _zoomDistance;
        private float _sensitivity;

        private RaycastHit _camHit;
        private RaycastHit _camHit2;

        private void Start()
        {
            _camDist = Camera.transform.localPosition;
            _zoomDistance = Manager.Data.CameraData.ZoomDefault;
            _camDist.z = -_zoomDistance;

            Cursor.lockState = Manager.Data.CameraData.SetCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = Manager.Data.CameraData.SetCursorVisible;
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
                _camDist.z = Mathf.Lerp(_camDist.z, -_zoomDistance, Time.deltaTime * Manager.Data.CameraData.ScrollDampening);
            }

            Camera.transform.localPosition = _camDist;

            GameObject obj = new GameObject();
            obj.transform.SetParent(Camera.transform.parent);
            obj.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                Camera.transform.localPosition.z - Manager.Data.CameraData.CollisionSensitivity);

            Vector3 direction = obj.transform.position - Camera.transform.position;
            direction.Normalize();
            Debug.DrawLine(Camera.transform.position, CameraCenter.transform.position + direction * Manager.Data.CameraData.CollisionSensitivity, Color.red);

            if (Physics.Linecast(CameraCenter.transform.position + direction * Manager.Data.CameraData.CollisionSensitivity, Camera.transform.position, out _camHit, ~Manager.Data.CameraData.PlayerLayer, QueryTriggerInteraction.Ignore))
            {
                Physics.Linecast(Camera.transform.position, CameraCenter.transform.position + direction * Manager.Data.CameraData.CollisionSensitivity, out _camHit2, ~Manager.Data.CameraData.PlayerLayer, QueryTriggerInteraction.Ignore);
                //Debug.Log("Distance : " + Vector3.Distance(_camHit.point, _camHit2.point));

                if (Vector3.Distance(_camHit.point, _camHit2.point) > Manager.Data.CameraData.MinCollisionThickness)
                {

                    Camera.transform.position = _camHit.point;

                    var localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                        Camera.transform.localPosition.z + Manager.Data.CameraData.CollisionSensitivity);

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
                                        FocusPoint.transform.position.y + Manager.Data.CameraData.YOffset, FocusPoint.transform.position.z);

            float xAngle = CameraCenter.transform.rotation.eulerAngles.x - _cameraMoveVector.y * _sensitivity / 2;
            float yAngle = CameraCenter.transform.rotation.eulerAngles.y + _cameraMoveVector.x * _sensitivity;

            var rotation = Quaternion.Euler(
                MathExtentions.ClampAngle(xAngle, Manager.Data.CameraData.YDownLimit, Manager.Data.CameraData.YUpLimit),
                yAngle,
                CameraCenter.transform.rotation.eulerAngles.z);

            CameraCenter.transform.rotation = rotation;
        }

        private void ScrollCamera()
        {
            if (!Manager.Data.CameraData.CanScroll) return;

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * Manager.Data.CameraData.ScrollSensitivity;
                scrollAmount *= _zoomDistance * 0.3f;
                _zoomDistance -= scrollAmount;
                _zoomDistance = Mathf.Clamp(_zoomDistance, Manager.Data.CameraData.ZoomMin, Manager.Data.CameraData.ZoomMax);
            }
        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            _sensitivity = manager.Inputs.IsMouse ? Manager.Data.CameraData.MouseSensitivity : Manager.Data.CameraData.ControllerSensitivity;
            float xSign = (manager.Inputs.IsMouse ? Manager.Data.CameraData.IsMouseXInverted : Manager.Data.CameraData.IsControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? Manager.Data.CameraData.IsMouseYInverted : Manager.Data.CameraData.IsControllerYInverted) ? -1 : 1;
            _cameraMoveVector = manager.Inputs.CameraMoveVector;
            _cameraMoveVector = new Vector3(_cameraMoveVector.x * xSign, _cameraMoveVector.y * ySign);
        }
    }
}