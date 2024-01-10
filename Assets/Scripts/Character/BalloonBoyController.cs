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
        [SerializeField] private float _airRefillSpeed;
        [SerializeField] private float _airRefillStart;

        [Header("Dash")]
        [Tooltip("The vertical speed of the dash at its start")] [SerializeField] private float _baseVerticalDashForce;
        [Tooltip("The lateral speed of the dash at its start")] [SerializeField] private float _baseLateralDashForce;
        [Tooltip("The speed at which the player loses velocity when stopping the dash")] [Range(0, 1)] [SerializeField] private float _dashDecel;
        [Tooltip("The speed at which the player loses velocity when touching the ground")] [Range(0, 1)] [SerializeField] private float _groundEnterDecel;
        [Tooltip("The force at which the player gains velocity when starting the dash")] [SerializeField] private float _accelerationDashForce;
        [Tooltip("The force at which the player loses velocity when stopping the dash")] [SerializeField] private float _deccelerationDashForce;
        [Tooltip("The speed at which the player gains velocity during the dash")] [Range(0, 1)] [SerializeField] private float _accelerationDashAccel;
        [Tooltip("The speed at which the player loses velocity after the dash")] [Range(0, 1)] [SerializeField] private float _deccelerationDashAccel;
        [Tooltip("The maximum vertical speed of the dash")] [SerializeField] private float _maxVerticalDashForce;
        [Tooltip("The maximum lateral speed of the dash")] [SerializeField] private float _maxLateralDashForce;

        [Header("Visuals")]
        [SerializeField] private RadialUI _airUI;
        [SerializeField] private GameObject _jumpFXPrefab;
        [SerializeField] private GameObject _floatFXPrefab;
        [SerializeField] private GameObject _balloonVisual;
        [SerializeField] private float _balloonScaleValue = 1f;
        [SerializeField] private float _balloonScaleTime = 1f;

        [Header("Sounds")]
        [SerializeField] private AudioClip _inflateSound;
        [SerializeField] private AudioSource _dashSound;
        [SerializeField] private float _dashSoundFadeSpeed;

        private int _jumps;
        private float _currentAir;
        private float _currentFillingAir;
        private float _airRefillTime;
        private bool _isFloating;
        private bool _isDashing;
        private bool _isFloatCanceled;
        private Vector3 _balloonOriginalScale;
        private Vector3 _currentDashDirection;
        private Vector3 _lateralInputDirection;
        private Vector3 _lastLateralInputDirection;
        private Vector3 _verticalInputDirection;
        private Vector3 _forwardInputDirection;
        private float _verticalDashForce;
        private float _lateralDashForce;

        private PlayerInputs _inputs;


        private void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void Start()
        {
            _jumps = _maxJumps;
            _currentAir = _maxAir;
            _currentFillingAir = 0;
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

            if (_currentAir <= 0 || _currentFillingAir > 0)
            {
                StartAirRefill();
                return;
            }
            if (!_isFloating) return;

            float airReductionSpeed = _airReductionSpeed;
            UpdateDashSpeed();
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

            UpdateSounds();
        }

        private void UpdateSounds()
        {
            int volume = _isDashing ? 1 : 0;

            if (AudioManager.Instance != null)
                AudioManager.Instance.FadeAudioSourceVolume(_dashSound, _dashSoundFadeSpeed, volume);
        }

        private void UpdateDashSpeed()
        {
            if (_isFloating && _isDashing)
            {
                _verticalDashForce = Mathf.Lerp(_verticalDashForce, _verticalDashForce + _accelerationDashForce, _accelerationDashAccel);
                _lateralDashForce = Mathf.Lerp(_lateralDashForce, _lateralDashForce + _accelerationDashForce, _accelerationDashAccel);
            } else
            {
                _verticalDashForce = Mathf.Lerp(_verticalDashForce, _verticalDashForce - _deccelerationDashForce, _deccelerationDashAccel);
                _lateralDashForce = Mathf.Lerp(_lateralDashForce, _lateralDashForce - _deccelerationDashForce, _deccelerationDashAccel);
            }
            _verticalDashForce = Mathf.Clamp(_verticalDashForce, _baseVerticalDashForce, _maxVerticalDashForce);
            _lateralDashForce = Mathf.Clamp(_lateralDashForce, _baseLateralDashForce, _maxLateralDashForce);

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
            if (_currentAir <= 0)
                _airUI.SetFillAmount(_currentFillingAir / _maxAir);
            else
                _airUI.SetFillAmount(_currentAir / _maxAir);
        }

        private void BalloonPump(InputAction.CallbackContext context)
        {
            if (_verticalInputDirection != Vector3.zero || _forwardInputDirection != Vector3.zero) return;

            if (_isFloatCanceled)
            {
                AfterFloatCancelJump();
            }

            _currentFillingAir = 0;
            _isFloating = true;
            _isDashing = true;
            _currentDashDirection = Vector3.zero;
            _isFloatCanceled = false;

            if (_currentAir <= 0) return;

            _balloonVisual.transform.DOScale(Vector3.one * _balloonScaleValue, _balloonScaleTime);

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

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_inflateSound);

            RefreshAir();

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

            // Sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClip(_inflateSound);

            CharacterControllerTest.Instance.SetFloatingCamera(true);
        }

        private void PlayerEnterGround()
        {
            _jumps = _maxJumps;
            RefreshAir();
            CharacterControllerTest.Instance.OnGroundEnter -= PlayerEnterGround;
            ResetBalloonScale();

        }

        private void ResetBalloonScale()
        {
            CharacterControllerTest.Instance.SetForce(Vector3.zero, _groundEnterDecel);
            _balloonVisual.transform.DOScale(_balloonOriginalScale, _balloonScaleTime);
            _currentDashDirection = Vector3.zero;
            _isFloating = false;
            _currentFillingAir = 0;
            _airRefillTime = _airRefillStart;
            CharacterControllerTest.Instance.SetFloatingCamera(false);

        }

        private void StartBalloonFloating(InputAction.CallbackContext context)
        {
            if (!_isFloating) return;
            if (_isFloatCanceled) return;

            _isDashing = true;
            _currentDashDirection = Vector3.zero;
        }

        private void StopBalloonFloating(InputAction.CallbackContext context)
        {
            if (_verticalInputDirection != Vector3.zero && _forwardInputDirection != Vector3.zero) return;

            _isDashing = false;
            _currentDashDirection = Vector3.zero;

            if (_currentAir <= 0) return;
            Vector3 force = _isFloatCanceled ? Vector3.zero : Vector3.up * _glideForce;
            CharacterControllerTest.Instance.SetForce(force, _dashDecel); // AddForce(-_currentDashDirection, _dashDecel);
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

        public void RefreshAir()
        {
            _currentAir = _maxAir;
        }

        private void StartAirRefill()
        {
            _airRefillTime -= Time.deltaTime;
            if (_airRefillTime > 0) return;

            _currentFillingAir += Time.deltaTime * _airRefillSpeed;
            _currentAir = _currentFillingAir;
            if (_currentFillingAir >= _maxAir)
            {
                RefreshAir();
                _currentFillingAir = 0;
            }
        }
    }
}