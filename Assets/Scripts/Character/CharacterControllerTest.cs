using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
                SwitchCamera(_canMove);
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
        [SerializeField] private float _gravityIncreaseByFrame;

        [Header("Ground Check")]
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _sphereRadius;
        [SerializeField] private LayerMask _playerLayer;

        [Header("Cameras")]
        [SerializeField] private GameObject _gameplayCamera;
        [SerializeField] private GameObject _buildingManagerCamera;
        [SerializeField] private GameObject _birdViewCamera;

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
        }

        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Player.Move.performed += StartMove;
            _inputs.Player.Move.canceled += StopMove;
            _inputs.Player.DistanceView.performed += BirdEyeView;
        }

        private void OnDisable()
        {
            _inputs.Disable();
            _inputs.Player.Move.performed -= StartMove;
            _inputs.Player.Move.canceled -= StopMove;
            _inputs.Player.DistanceView.performed -= BirdEyeView;


        }

        private void Update()
        {
            if (!CanMove || _isViewing)
            {
                SetAnimation(Vector3.zero);
                return;
            }

            Vector3 moveDirection = UnityEngine.Camera.main.transform.forward * _moveVector.z + UnityEngine.Camera.main.transform.right * _moveVector.x;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            SetAnimation(moveDirection);
            _characterController.Move(moveDirection * Time.deltaTime);

            var grounded = IsGrounded;
            IsGrounded = Physics.SphereCastNonAlloc(transform.position, _sphereRadius, Vector3.down, _groundHitResults, _groundCheckDistance, _playerLayer) > 0;
            if (grounded != IsGrounded && IsGrounded)
            {
                LastGround = _groundHitResults[0];
                OnGroundEnter?.Invoke();
                CurrentGravity = BaseGravity;
            }
        }

        private void StartMove(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            _moveVector = new Vector3(xPosition * _speed, 0, zPosition * _speed);
        }

        private void StopMove(InputAction.CallbackContext context)
        {
            _moveVector = Vector3.zero;
        }

        void FixedUpdate()
        {
            if (!IsGrounded && _balloonStateManager.GetState() == _balloonStateManager.BalloonHammer)
            {
                CurrentGravity = Mathf.Clamp(CurrentGravity + _gravityIncreaseByFrame, BaseGravity, MaxGravity);
            }

            Vector3 gravity = new Vector3(Force.x, Force.y - CurrentGravity, Force.z);

            _characterController.Move(gravity * Time.deltaTime);

            var forceSign = Mathf.Sign(CurrentForce.x) * Mathf.Sign(CurrentForce.x) * Mathf.Sign(CurrentForce.x);
            Force = Vector3.Lerp(Force, CurrentForce, _lerpValue);
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

        private void ActivateCamera(GameObject camera)
        {
            camera.SetActive(false);
            camera.SetActive(true);
        }

        private void SwitchCamera(bool canMove)
        {
            GameObject camera = canMove ? _gameplayCamera : _buildingManagerCamera;
            ActivateCamera(camera);
        }

        private void BirdEyeView(InputAction.CallbackContext context)
        {
            _isViewing = !_isViewing;
            _birdViewCamera.SetActive(_isViewing);
        }
    }
}
