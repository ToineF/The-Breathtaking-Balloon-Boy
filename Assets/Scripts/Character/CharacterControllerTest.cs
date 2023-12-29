using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections.Generic;
using BlownAway.GPE;
using System.Linq;

namespace BlownAway.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerTest : MonoBehaviour
    {
        public static CharacterControllerTest Instance;

        public Action OnGroundEnter;

        public bool CanMove
        {
            get => _canMove; set
            {
                _canMove = value;
                //SwitchCamera(_canMove);
            }
        }

        [ReadOnly] public Vector3 Force;
        [ReadOnly] public Vector3 CurrentForce;
        [ReadOnly] public bool IsGrounded;
        [HideInInspector] public RaycastHit LastGround;


        [Header("Walk")]
        [SerializeField] private float _speed;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animatorWalkBool;

        [Header("Gravity")]
        [ReadOnly] public float CurrentGravity;
        [SerializeField] private float BaseGravity;
        [SerializeField] private float MaxGravity;
        [SerializeField] private float FloatingGravity;
        [SerializeField] private float _gravityIncreaseByFrame;

        [Header("Ground Check")]
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _sphereRadius;
        [SerializeField] private LayerMask _playerLayer;

        [Header("Cameras")]
        [SerializeField] private EntityCamera _gameplayCamera;
        [SerializeField] private EntityCamera _buildingManagerCamera;
        [SerializeField] private EntityCamera _birdViewCamera;
        [SerializeField] private EntityCamera _floatingCamera;

        // References
        private bool _canMove;
        private PlayerInputs _inputs;
        private CharacterController _characterController;
        private BalloonStateManager _balloonStateManager;

        // Walk
        private Vector3 _moveVector = Vector3.zero;
        private float _lerpValue;

        // Ground Check
        private RaycastHit[] _groundHitResults;
        private IEnumerator _currentForceCoroutine;

        // Camera
        private bool _isViewing;
        private EntityCamera _currentCamera;
        private Vector2 _currentCameraAngle;

        // External
        private Dictionary<GameObject, ForceData> _additionnalForces = new Dictionary<GameObject, ForceData>();

        private void Awake()
        {
            Instance = this;
            _inputs = new PlayerInputs();
        }

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _balloonStateManager = GetComponent<BalloonStateManager>();
            CurrentForce = Force;
            _groundHitResults = new RaycastHit[2];
            CurrentGravity = BaseGravity;
            CanMove = true;
            ActivateCamera(_gameplayCamera);
        }

        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Player.Move.performed += StartMove;
            _inputs.Player.Move.canceled += StopMove;
            _inputs.Player_Archive.DistanceView.performed += BirdEyeView;
            _inputs.Player.CameraMove.performed += CameraMove;
        }

        private void OnDisable()
        {
            _inputs.Disable();
            _inputs.Player.Move.performed -= StartMove;
            _inputs.Player.Move.canceled -= StopMove;
            _inputs.Player_Archive.DistanceView.performed -= BirdEyeView;
            _inputs.Player.CameraMove.performed -= CameraMove;
        }

        private void Update()
        {
            if (Time.timeScale == 0) return;

            if (!CanMove || _isViewing)
            {
                SetAnimation(Vector3.zero);
                UpdateCamera();

                return;
            }

            Vector3 moveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)) * _moveVector.z + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * _moveVector.x).normalized;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            SetAnimation(moveDirection);
            _characterController.Move(moveDirection * _speed * Time.deltaTime);
            UpdateCamera();


            var grounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(transform.position, _sphereRadius, Vector3.down, _groundHitResults, _groundCheckDistance, _playerLayer) > 0;
            if (grounded != IsGrounded && IsGrounded)
            {
                LastGround = _groundHitResults[0];
                OnGroundEnter?.Invoke();
                CurrentGravity = BaseGravity;
            }
            SetGravity();
        }


        private void SetGravity()
        {
            if (!IsGrounded) // && _balloonStateManager.GetState() == _balloonStateManager.BalloonHammer
            {
                CurrentGravity = Mathf.Clamp(CurrentGravity + _gravityIncreaseByFrame, BaseGravity, MaxGravity);
            }

            Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }
            Vector3 gravity = - CurrentGravity * new Vector3(0, 1, 0);
            Vector3 allForces = Force + additionalForces + gravity;

            _characterController.Move(allForces * Time.deltaTime);

            // var forceSign = Mathf.Sign(CurrentForce.x) * Mathf.Sign(CurrentForce.x) * Mathf.Sign(CurrentForce.x);
            Force = Vector3.Lerp(Force, CurrentForce, _lerpValue);
        }

        private void UpdateCamera()
        {
            float YPosition = _currentCamera.YOffset + _currentCameraAngle.y;
            Vector3 cameraVector = new Vector3((float)Math.Cos(_currentCameraAngle.x), YPosition, (float)Math.Sin(_currentCameraAngle.x)).normalized * int.MaxValue;
            Vector3 newPosition = transform.position + cameraVector;
            _currentCamera.transform.position = newPosition;

            _currentCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = _currentCamera.PositionOffset;
            _currentCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = _currentCamera.CameraDistance;
        }

        public void StartMove(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            _moveVector = new Vector3(xPosition, 0, zPosition);
        }

        public void StopMove(InputAction.CallbackContext context)
        {
            _moveVector = Vector3.zero;
        }

        private void SetAnimation(Vector3 moveVector)
        {
            _animator.SetBool(_animatorWalkBool, moveVector != Vector3.zero);
            transform.LookAt(transform.position + moveVector);
        }

        public void AddForce(Vector3 force, float lerp)
        {
            CurrentForce += force;
            _lerpValue = lerp;
        }

        public void SetForce(Vector3 force, float lerp)
        {
            if (_currentForceCoroutine != null) StopCoroutine(_currentForceCoroutine);
            CurrentForce = force;
            _lerpValue = lerp;
        }

        public void SetForceForTime(Vector3 force, float time, float startLerp, float endLerp)
        {
            SetForce(force, startLerp);
            _currentForceCoroutine = WaitForAction(time, () => SetForce(CurrentForce - force, endLerp));
            StartCoroutine(_currentForceCoroutine);
        }

        private IEnumerator WaitForAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        private void ActivateCamera(EntityCamera camera)
        {
            _currentCamera = camera;
            camera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
        }

        private void SwitchCamera(bool canMove)
        {
            //EntityCamera camera = canMove ? _gameplayCamera : _buildingManagerCamera;
            //ActivateCamera(camera);
        }

        private void BirdEyeView(InputAction.CallbackContext context)
        {
            if (!CanMove) return;
            EntityCamera camera = _isViewing ? _gameplayCamera : _birdViewCamera;
            _isViewing = !_isViewing;
            //ActivateCamera(camera);
        }

        public void SetFloatingCamera(bool isActive)
        {
            if (!CanMove || _isViewing) return;
            EntityCamera camera = isActive ? _floatingCamera : _gameplayCamera;
            //ActivateCamera(camera);
        }

        private void CameraMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>() * _currentCamera.MouseSpeed;
            float xSign = _currentCamera.IsXInverted ? -1 : 1;
            float ySign = _currentCamera.IsYInverted ? -1 : 1;
            _currentCameraAngle += new Vector2(value.x * xSign, value.y * ySign);
            _currentCameraAngle.y = Math.Clamp(_currentCameraAngle.y, -_currentCamera.YDeadZone, _currentCamera.YDeadZone);
        }

        public void AddAdditionalForce(GameObject parent, Vector3 force, float lerp)
        {
            if (!_additionnalForces.ContainsKey(parent))
                _additionnalForces.Add(parent, new ForceData(force, lerp));
            else
            {
                _additionnalForces[parent].TargetForce = force;
                _additionnalForces[parent].ForceLerp = lerp;
            }
        }

        public void RemoveAdditionalForce(GameObject parent)
        {
            _additionnalForces.Remove(parent);
        }
    }
}
