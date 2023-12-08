using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

namespace BlownAway.Player
{
    public class BalloonBoyController : MonoBehaviour
    {

        [Header("Controller Type")]
        [SerializeField] private ControllerType _type;

        [Header("Glide")]
        [SerializeField] private int _maxJumps;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpTime;
        [Range(0, 1)] [SerializeField] private float _jumpAccel;
        [Range(0, 1)] [SerializeField] private float _jumpDecel;
        [SerializeField] private float _glideForce;
        [Range(0, 1)] [SerializeField] private float _glideAccel;

        [Header("Air")]
        [SerializeField] private float _maxAir;
        [SerializeField] private float _airReductionSpeed;
        [SerializeField] private float _airDashReductionSpeed;

        [Header("Dash")]
        [SerializeField] private float _verticalDashForce;
        [SerializeField] private float _lateralDashForce;
        [Range(0, 1)] [SerializeField] private float _dashDecel;

        [Header("Visuals")]
        [SerializeField] private RadialUI _airUI;
        [SerializeField] private GameObject _jumpFXPrefab;
        [SerializeField] private GameObject _floatFXPrefab;
        [SerializeField] private GameObject _balloonVisual;
        [SerializeField] private float _balloonScaleValue = 1f;
        [SerializeField] private float _balloonScaleTime = 1f;

        private int _jumps;
        private float _currentAir;
        private bool _isFloating;
        private bool _isDashing;
        private bool _isFloatCanceled;
        private Vector3 _balloonOriginalScale;
        private Vector3 _currentDashDirection;
        private Vector3 _lateralInputDirection;
        private Vector3 _lastLateralInputDirection;
        private Vector3 _verticalInputDirection;
        private Vector3 _forwardInputDirection;

        private PlayerInputs _inputs;


        private void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void Start()
        {
            _jumps = _maxJumps;
            _currentAir = _maxAir;
            _balloonOriginalScale = _balloonVisual.transform.localScale;
            _currentDashDirection = Vector3.zero;
            _lateralInputDirection = Vector3.zero;
            _lastLateralInputDirection = Vector3.zero;
            _verticalInputDirection = Vector3.zero;
        }

        private void OnEnable()
        {
            ControllerTypeEnable();
        }

        private void ControllerTypeEnable()
        {
            _inputs.Enable();

            switch (_type)
            {
                case ControllerType.Type1:
                    _inputs.Player.Action.performed += BalloonPump;
                    _inputs.Player.Action.performed += GetMoveInputUp;
                    //_inputs.Player.Action.performed += StartBalloonFloating;
                    _inputs.Player.Action.canceled += StopBalloonFloating;
                    _inputs.Player.Action.canceled += ResetVerticalMoveInput;
                    _inputs.Player.Move.performed += GetMoveValue;
                    _inputs.Player.Move.performed += ResetLateralMoveInput;
                    _inputs.Player.Move.canceled += GetMoveValue;
                    _inputs.Player.SecondaryAction.performed += CancelBalloonFloating;
                    break;
                case ControllerType.Type2:
                    _inputs.Player_1.LateralPropulsion.performed += BalloonPump;
                    _inputs.Player_1.LateralPropulsion.performed += GetForwardInput;
                    _inputs.Player_1.LateralPropulsion.canceled += ResetLateralMoveInput;
                    _inputs.Player_1.UpPropulsion.performed += BalloonPump;
                    _inputs.Player_1.UpPropulsion.performed += GetMoveInputUp;
                    _inputs.Player_1.UpPropulsion.canceled += ResetVerticalMoveInput;
                    _inputs.Player_1.DownPropulsion.performed += GetMoveInputDown;
                    _inputs.Player_1.DownPropulsion.performed += StartBalloonFloating;
                    _inputs.Player_1.DownPropulsion.canceled += ResetVerticalMoveInput;
                    _inputs.Player_1.Move.performed += GetMoveValue;
                    _inputs.Player_1.Move.canceled += GetMoveValue;
                    _inputs.Player_1.CancelPropulsion.performed += CancelBalloonFloating;
                    break;
                case ControllerType.Type3:
                    break;
                default:
                    _inputs.Player.Action.performed += BalloonPump;
                    //_inputs.Player.Action.performed += StartBalloonFloating;
                    _inputs.Player.Action.canceled += StopBalloonFloating;
                    _inputs.Player.Move.performed += GetMoveValue;
                    _inputs.Player.Move.canceled += GetMoveValue;
                    _inputs.Player.SecondaryAction.performed += CancelBalloonFloating;
                    break;
            }

        }

        private void OnDisable()
        {
            ControllerTypeDisable();
        }

        private void ControllerTypeDisable()
        {
            _inputs.Disable();

            switch (_type)
            {
                case ControllerType.Type1:
                    _inputs.Player.Action.performed -= BalloonPump;
                    _inputs.Player.Action.performed -= GetMoveInputUp;
                    //_inputs.Player.Action.performed -= StartBalloonFloating;
                    _inputs.Player.Action.canceled -= StopBalloonFloating;
                    _inputs.Player.Action.canceled -= ResetVerticalMoveInput;
                    _inputs.Player.Move.performed -= GetMoveValue;
                    _inputs.Player.Move.performed -= ResetLateralMoveInput;
                    _inputs.Player.Move.canceled -= GetMoveValue;
                    _inputs.Player.SecondaryAction.performed -= CancelBalloonFloating;
                    break;
                case ControllerType.Type2:
                    _inputs.Player_1.LateralPropulsion.performed -= BalloonPump;
                    _inputs.Player_1.LateralPropulsion.performed -= GetForwardInput;
                    _inputs.Player_1.LateralPropulsion.canceled -= ResetLateralMoveInput;
                    _inputs.Player_1.UpPropulsion.performed -= BalloonPump;
                    _inputs.Player_1.UpPropulsion.performed -= GetMoveInputUp;
                    _inputs.Player_1.UpPropulsion.canceled -= ResetVerticalMoveInput;
                    _inputs.Player_1.DownPropulsion.performed -= GetMoveInputDown;
                    _inputs.Player_1.DownPropulsion.performed -= StartBalloonFloating;
                    _inputs.Player_1.DownPropulsion.canceled -= ResetVerticalMoveInput;
                    _inputs.Player_1.Move.performed -= GetMoveValue;
                    _inputs.Player_1.Move.canceled -= GetMoveValue;
                    _inputs.Player_1.CancelPropulsion.performed -= CancelBalloonFloating;
                    break;
                case ControllerType.Type3:
                    break;
                default:
                    _inputs.Player.Action.performed -= BalloonPump;
                    _inputs.Player.Action.canceled -= StopBalloonFloating;
                    _inputs.Player.Move.performed -= GetMoveValue;
                    _inputs.Player.Move.canceled -= GetMoveValue;
                    _inputs.Player.SecondaryAction.performed -= CancelBalloonFloating;
                    break;
            }
        }

        private void Update()
        {
            UpdateUI();

            if (!_isFloating) return;
            if (_currentAir <= 0) return;

            float airReductionSpeed = _airReductionSpeed;
            if (_isDashing)
            {
                airReductionSpeed += _airDashReductionSpeed;
                Vector3 newDashdirection = SetNewDashDirection();
                CharacterControllerTest.Instance.AddForce(newDashdirection - _currentDashDirection, _jumpDecel);
                _currentDashDirection = newDashdirection;

                Collider collider = CharacterControllerTest.Instance.GetComponent<Collider>();
                Instantiate(_floatFXPrefab, collider.bounds.center - collider.bounds.extents.y * newDashdirection.normalized, _floatFXPrefab.transform.rotation);
            }
            _currentAir -= airReductionSpeed * Time.deltaTime;
            if (_currentAir <= 0) ResetBalloonScale();
        }

        private Vector3 SetNewDashDirection()
        {
            Vector3 newDashdirection;
            if (_type == ControllerType.Type1)
            {
                Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * _lateralInputDirection.y + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * _lateralInputDirection.x).normalized;

                newDashdirection = (lateralMoveDirection == Vector3.zero) ? Vector3.up * _verticalDashForce : lateralMoveDirection * _lateralDashForce; // this one for type 2 & 3
            }
            else
            {
                Vector3 lateralMoveDirection = (Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized * _lastLateralInputDirection.y + Vector3.Scale(UnityEngine.Camera.main.transform.right, new Vector3(1, 0, 1)) * _lastLateralInputDirection.x).normalized;

                newDashdirection = Vector3.Scale(lateralMoveDirection, _forwardInputDirection).normalized * _lateralDashForce + _verticalInputDirection * _verticalDashForce;
            }
            return newDashdirection;
        }

        private void UpdateUI()
        {
            _airUI.SetFillAmount(_currentAir / _maxAir);
        }

        private void BalloonPump(InputAction.CallbackContext context)
        {
            if (_verticalInputDirection != Vector3.zero || _forwardInputDirection != Vector3.zero) return;

            if (_isFloatCanceled)
            {
                AfterFloatCancelJump();
            }

            _isFloating = true;
            _isDashing = true;
            _currentDashDirection = Vector3.zero;
            _isFloatCanceled = false;

            if (_jumps <= 0) return;

            CharacterControllerTest.Instance.OnGroundEnter += PlayerEnterGround;
            _jumps--;


            Vector3 hit = Vector3.up * _jumpForce;
            float startLerp = _jumpAccel;
            float endLerp = _jumpDecel;
            float time = _jumpTime;
            CharacterControllerTest.Instance.SetForceForTime(hit, time, startLerp, endLerp);

            Vector3 glideForce = Vector3.up * _glideForce;
            float glideAccel = _glideAccel;
            CharacterControllerTest.Instance.AddForce(glideForce, glideAccel);

            Collider collider = CharacterControllerTest.Instance.GetComponent<Collider>();
            Instantiate(_jumpFXPrefab, collider.bounds.center - collider.bounds.extents.y * Vector3.up, _jumpFXPrefab.transform.rotation);

            // Air
            _currentAir = _maxAir;

            _balloonVisual.transform.DOScale(Vector3.one * _balloonScaleValue, _balloonScaleTime);

            CharacterControllerTest.Instance.SetFloatingCamera(true);
        }

        private void AfterFloatCancelJump()
        {
            if (_currentAir <= 0) return;

            _currentDashDirection = Vector3.zero;
            _isFloating = true;
            _isDashing = true;

            Vector3 glideForce = Vector3.up * _glideForce;
            float glideAccel = _glideAccel;
            CharacterControllerTest.Instance.AddForce(glideForce, glideAccel);

            _balloonVisual.transform.DOScale(Vector3.one * _balloonScaleValue, _balloonScaleTime);

            CharacterControllerTest.Instance.SetFloatingCamera(true);
        }

        private void PlayerEnterGround()
        {
            _jumps = _maxJumps;
            _currentAir = _maxAir;
            CharacterControllerTest.Instance.OnGroundEnter -= PlayerEnterGround;
            ResetBalloonScale();

        }

        private void ResetBalloonScale()
        {
            CharacterControllerTest.Instance.SetForce(Vector3.zero, 1);
            _balloonVisual.transform.DOScale(_balloonOriginalScale, _balloonScaleTime);
            _currentDashDirection = Vector3.zero;
            _isFloating = false;
            CharacterControllerTest.Instance.SetFloatingCamera(false);

        }

        private void StartBalloonFloating(InputAction.CallbackContext context)
        {
            _isDashing = true;
            _currentDashDirection = Vector3.zero;
        }

        private void StopBalloonFloating(InputAction.CallbackContext context)
        {
            if (_verticalInputDirection != Vector3.zero && _forwardInputDirection != Vector3.zero) return;

            _isDashing = false;
            _currentDashDirection = Vector3.zero;

            if (_currentAir <= 0) return;
            CharacterControllerTest.Instance.SetForce(Vector3.up * _glideForce, _dashDecel); // AddForce(-_currentDashDirection, _dashDecel);
        }

        private void CancelBalloonFloating(InputAction.CallbackContext context)
        {
            _isDashing = false;
            _isFloating = false;
            _isFloatCanceled = true;
            ResetBalloonScale();
        }

        private void GetMoveValue(InputAction.CallbackContext context)
        {
            _lateralInputDirection = context.ReadValue<Vector2>();
            if (_lateralInputDirection != Vector3.zero ) _lastLateralInputDirection  = _lateralInputDirection;
        }

        private void GetMoveInputUp(InputAction.CallbackContext context)
        {
            _verticalInputDirection = Vector2.up;
        }

        private void GetMoveInputDown(InputAction.CallbackContext context)
        {
            _verticalInputDirection = Vector2.down;
        }
        private void ResetVerticalMoveInput(InputAction.CallbackContext context)
        {
            StopBalloonFloating(context);
            _verticalInputDirection = Vector2.zero;
        }

        private void ResetLateralMoveInput(InputAction.CallbackContext context)
        {
            StopBalloonFloating(context);
            _forwardInputDirection = Vector2.zero;
        }

        private void GetForwardInput(InputAction.CallbackContext context)
        {
            _forwardInputDirection = new Vector3(1, 0, 1);
        }
    }
}