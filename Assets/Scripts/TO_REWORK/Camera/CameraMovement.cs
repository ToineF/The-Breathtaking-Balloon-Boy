using UnityEngine;
using AntoineFoucault.Utilities;
using BlownAway.Character;

namespace BlownAway.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        // LINK THIS TO THE PLAYER CLASS
        // MAKE A SCRIPTABLE OBJECT OF THE DATA FOR THE GAME DESIGNERS

        [SerializeField] GameObject Player;
        [SerializeField] GameObject CameraCenter;
        [SerializeField] float _yOffset = 1f;

        //[Header("Mouse")]
        [SerializeField, Tooltip("The influence of the mouse on the camera speed")] private float _mouseSensitivity = 1;
        [SerializeField, Tooltip("Is the X-Axis Inverted")] private bool _isMouseXInverted;
        [SerializeField, Tooltip("Is the Y-Axis Inverted")] private bool _isMouseYInverted;

        //[Header("Controller")]
        [SerializeField, Tooltip("The influence of the controller on the camera speed")] private float _controllerSensitivity = 1;
        [SerializeField, Tooltip("Is the X-Axis Inverted")] private bool _isControllerXInverted;
        [SerializeField, Tooltip("Is the Y-Axis Inverted")] private bool _isControllerYInverted;

        [SerializeField] UnityEngine.Camera Camera;
        [SerializeField] LayerMask PlayerLayer;

        [SerializeField] float scrollSensitivity = 5f;
        [SerializeField] float scrollDampening = 6f;

        [SerializeField] float zoomMin = 3.5f;
        [SerializeField] float zoomMax = 15f;
        [SerializeField] float zoomDefault = 10f;
        [SerializeField] float zoomDistance;

        [SerializeField] float collisionSensitivity = 4.5f;
        [SerializeField] float minCollisionDistance = 1.5f;

        [SerializeField] float yUpLimit = 89.9f;
        [SerializeField] float yDownLimit = -89.9f;

        private CharacterManager _manager;
        private Vector3 _cameraMoveVector;

        private RaycastHit _camHit;
        private RaycastHit _camHit2;
        private Vector3 _camDist;
        private float _sensitivity;

        private void Start()
        {
            _camDist = Camera.transform.localPosition;
            zoomDistance = zoomDefault;
            _camDist.z = -zoomDistance;

            _manager = GameManager.Instance.CharacterManager;

            //Cursor.visible = false;
        }

        private void LateUpdate()
        {
            UpdateCameraAngle(_manager);
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            CameraCenter.transform.position = new Vector3(Player.transform.position.x,
                            Player.transform.position.y + _yOffset, Player.transform.position.z);

            float xAngle = CameraCenter.transform.rotation.eulerAngles.x - _cameraMoveVector.y * _sensitivity / 2;
            float yAngle = CameraCenter.transform.rotation.eulerAngles.y + _cameraMoveVector.x * _sensitivity;

            var rotation = Quaternion.Euler(
                MathExtentions.ClampAngle(xAngle, yDownLimit, yUpLimit),
                yAngle,
                CameraCenter.transform.rotation.eulerAngles.z);

            CameraCenter.transform.rotation = rotation;

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
                scrollAmount *= zoomDistance * 0.3f;
                zoomDistance -= scrollAmount;
                zoomDistance = Mathf.Clamp(zoomDistance, zoomMin, zoomMax);
            }

            if (_camDist.z != -zoomDistance)
            {
                _camDist.z = Mathf.Lerp(_camDist.z, -zoomDistance, Time.deltaTime * scrollDampening);
            }

            Camera.transform.localPosition = _camDist;

            GameObject obj = new GameObject();
            obj.transform.SetParent(Camera.transform.parent);
            obj.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                Camera.transform.localPosition.z - collisionSensitivity);

            Vector3 direction = obj.transform.position - Camera.transform.position;
            direction.Normalize();
            Debug.DrawLine(Camera.transform.position, CameraCenter.transform.position + direction * collisionSensitivity, Color.red);

            if (Physics.Linecast(CameraCenter.transform.position + direction * collisionSensitivity, Camera.transform.position, out _camHit, ~PlayerLayer, QueryTriggerInteraction.Ignore))
            {
                Physics.Linecast(Camera.transform.position, CameraCenter.transform.position + direction * collisionSensitivity, out _camHit2, ~PlayerLayer, QueryTriggerInteraction.Ignore);
                //Debug.Log("Distance : " + Vector3.Distance(_camHit.point, _camHit2.point));

                if (Vector3.Distance(_camHit.point, _camHit2.point) > minCollisionDistance)
                {

                    Camera.transform.position = _camHit.point;

                    var localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y,
                        Camera.transform.localPosition.z + collisionSensitivity);

                    Camera.transform.localPosition = localPosition;
                }

            }

            Destroy(obj);

            if (Camera.transform.localPosition.z > -1f)
            {
                Camera.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y, -1f);
            }
        }

        public void UpdateCameraAngle(CharacterManager manager) // Late Update
        {
            _sensitivity = manager.Inputs.IsMouse ? _mouseSensitivity : _controllerSensitivity;
            float xSign = (manager.Inputs.IsMouse ? _isMouseXInverted : _isControllerXInverted) ? -1 : 1;
            float ySign = (manager.Inputs.IsMouse ? _isMouseYInverted : _isControllerYInverted) ? -1 : 1;
            _cameraMoveVector = manager.Inputs.CameraMoveVector;
            _cameraMoveVector = new Vector3(_cameraMoveVector.x * xSign, _cameraMoveVector.y * ySign);
            //_currentCameraAngle += new Vector2(manager.Inputs.CameraMoveVector.x * xSign, manager.Inputs.CameraMoveVector.y * ySign % 360) * sensitivity;
            //_currentCameraAngle.y = Math.Clamp(_currentCameraAngle.y, -_cameraParams.YDeadZone, _cameraParams.YDeadZone);
        }
    }
}