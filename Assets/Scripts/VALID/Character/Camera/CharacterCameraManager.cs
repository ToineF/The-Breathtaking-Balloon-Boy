using UnityEngine;
using AntoineFoucault.Utilities;
using BlownAway.Character;
using DG.Tweening;

namespace BlownAway.Camera
{
    public class CharacterCameraManager : CharacterSubComponent
    {
        public bool IsMovable { get; set; } = true;


        [field: Header("References")]
        [field: SerializeField] public GameObject Camera { get; private set; }
        [SerializeField] GameObject FocusPoint;
        [SerializeField] GameObject CameraCenter;
        [SerializeField] Transform TopDownPoint;

        private Vector3 _cameraMoveVector;
        private Vector3 _lastRotation;

        private Vector3 _camDist;
        private float _zoomDistance;
        private float _sensitivity;

        private RaycastHit _camHit;
        private RaycastHit _camHit2;

        private bool _canMoveCamera = true;
        private bool _cameraIsTopDown = false;
        private float _cameraCanMoveTimer;

        protected override void StartScript(CharacterManager manager)
        {
            _camDist = Camera.transform.localPosition;
            _zoomDistance = manager.Data.CameraData.ZoomDefault;
            _camDist.z = -_zoomDistance;

            _canMoveCamera = true;

            SetCursorVisible(manager.Data.CameraData.SetCursorVisible);

            CenterCamera(manager, new Vector3(0, manager.CharacterVisual.transform.eulerAngles.y, 0), 0);

            CameraCenter.transform.position = new Vector3(FocusPoint.transform.position.x, FocusPoint.transform.position.y + Manager.Data.CameraData.YOffsetUpLimit, FocusPoint.transform.position.z);

        }

        private void LateUpdate()
        {
            if (!IsMovable) return;

            UpdateCameraAngle(Manager);
            UpdateCameraPosition(Manager);
        }

        private void Update()
        {
            if (!IsMovable) return;

            CheckForCameraTopDown(Manager);

            if (!_canMoveCamera) return;

            CheckForCameraCenter(Manager);
        }

        private void UpdateCameraPosition(CharacterManager manager)
        {
            CheckForCameraCanMove();

            SetCameraAngle(manager);

            if (!_canMoveCamera) return;

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
                Camera.transform.localPosition.z - Manager.Data.CameraData.CollisionRaycastOffset);

            Vector3 direction = obj.transform.position - Camera.transform.position;
            direction.Normalize();
            Debug.DrawLine(Camera.transform.position, CameraCenter.transform.position + direction * Manager.Data.CameraData.CollisionRaycastOffset, Color.red);

            if (Physics.Linecast(CameraCenter.transform.position + direction * Manager.Data.CameraData.CollisionRaycastOffset, Camera.transform.position, out _camHit, ~Manager.Data.CameraData.PlayerLayer, QueryTriggerInteraction.Ignore))
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

        private void SetCameraAngle(CharacterManager manager)
        {
            float normalizedDistance = (Vector3.Distance(FocusPoint.transform.position, Camera.transform.position) - Manager.Data.CameraData.ZoomMin) / (Manager.Data.CameraData.ZoomMax - Manager.Data.CameraData.ZoomMin);
            float yOffset = Mathf.Lerp(Manager.Data.CameraData.YOffsetDownLimit, Manager.Data.CameraData.YOffsetUpLimit, normalizedDistance);
            Vector3 focusPosition = new Vector3(FocusPoint.transform.position.x, FocusPoint.transform.position.y + yOffset, FocusPoint.transform.position.z);
            CameraCenter.transform.position = Vector3.Lerp(CameraCenter.transform.position, focusPosition, manager.Data.CameraData.CameraFollowLerpTime);

            if (!_canMoveCamera || _cameraIsTopDown) return;

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
                _zoomDistance = Mathf.Clamp(_zoomDistance, Manager.Data.CameraData.ScrollZoomMin, Manager.Data.CameraData.ScrollZoomMax);
            }
        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            _sensitivity = manager.Inputs.IsMouse ? manager.Data.CameraData.MouseSensitivityGameplay : manager.Data.CameraData.ControllerSensitivityGameplay;
            float xSign = (manager.Inputs.IsMouse ? manager.Data.CameraData.IsMouseXInverted : manager.Data.CameraData.IsControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? manager.Data.CameraData.IsMouseYInverted : manager.Data.CameraData.IsControllerYInverted) ? -1 : 1;
            _cameraMoveVector = manager.Inputs.CameraMoveVector;
            _cameraMoveVector = new Vector3(_cameraMoveVector.x * xSign, _cameraMoveVector.y * ySign);
            //if (_cameraMoveVector != Vector3.zero) CameraCenter.transform.DOKill();
        }

        private void CheckForCameraCenter(CharacterManager manager)
        {
            if (manager.Inputs.CameraCenter)
            {
                CenterCamera(manager, new Vector3(0, manager.CharacterVisual.transform.eulerAngles.y, 0), manager.Data.CameraData.CameraCenterLerpTime);
            }
        }

        private void CenterCamera(CharacterManager manager, Vector3 eulerAngles, float rotateTime)
        {
            _canMoveCamera = false;
            _cameraCanMoveTimer = rotateTime;
            CameraCenter.transform.DORotate(eulerAngles, rotateTime);
        }

        public void CenterCameraImmediate(CharacterManager manager)
        {
            CameraCenter.transform.localEulerAngles = Vector3.zero;
        }

        private void CheckForCameraTopDown(CharacterManager manager)
        {
            if (!manager.Data.CameraData.CanUseCameraTopDown) return;

            if (manager.Data.CameraData.IsCameraTopDownHold)
                CameraTopDownHold(manager);
            else
                CameraTopDownToggle(manager);
        }

        private void CameraTopDownHold(CharacterManager manager)
        {
            if (manager.Inputs.CameraTopDownPressed)
            {
                _cameraIsTopDown = true;
                if (_canMoveCamera) _lastRotation = Camera.transform.eulerAngles;
                CenterCamera(manager, new Vector3(TopDownPoint.eulerAngles.x, Camera.transform.eulerAngles.y, 0), manager.Data.CameraData.CameraTopDownStartLerpTime);
            }
            else if (manager.Inputs.CameraTopDownReleased)
            {
                _cameraIsTopDown = false;
                CenterCamera(manager, _lastRotation, manager.Data.CameraData.CameraTopDownEndLerpTime);
            }
        }

        private void CameraTopDownToggle(CharacterManager manager)
        {
            if (!manager.Inputs.CameraTopDownPressed) return;

            _cameraIsTopDown = !_cameraIsTopDown;

            if (_cameraIsTopDown)
            {
                if (_canMoveCamera) _lastRotation = Camera.transform.eulerAngles;
                CenterCamera(manager, new Vector3(TopDownPoint.eulerAngles.x, Camera.transform.eulerAngles.y, 0), manager.Data.CameraData.CameraTopDownStartLerpTime);
            }
            else
            {
                CenterCamera(manager, _lastRotation, manager.Data.CameraData.CameraTopDownEndLerpTime);
            }
        }

        private void CheckForCameraCanMove()
        {
            if (_canMoveCamera) return;

            _cameraCanMoveTimer -= Time.deltaTime;

            if (_cameraCanMoveTimer < 0)
            {
                _canMoveCamera = true;
            }
        }

        public void ReactivateCamera()
        {
            Camera.SetActive(false);
            Camera.SetActive(true);
        }

        public void SetCursorVisible(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isVisible;
        }
    }
}