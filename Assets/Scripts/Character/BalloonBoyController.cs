using BlownAway.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class BalloonBoyController : MonoBehaviour
{
    [Header("Glide")]
    [SerializeField] private int _maxJumps;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpTime;
    [Range(0, 1)][SerializeField] private float _jumpAccel;
    [Range(0, 1)][SerializeField] private float _jumpDecel;
    [SerializeField] private float _glideForce;
    [Range(0, 1)] [SerializeField] private float _glideAccel;

    [Header("Air")]
    [SerializeField] private float _maxAir;
    [SerializeField] private float _airReductionSpeed;
    [SerializeField] private float _airDashReductionSpeed;

    [Header("Dash")]
    [SerializeField] private float _verticalDashForce;
    [SerializeField] private float _lateralDashForce;
    [Range(0, 1)][SerializeField] private float _dashDecel;

    [Header("Visuals")]
    [SerializeField] private Image _airUI;
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
    private Vector3 _lateralMovementDirection;

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
        _lateralMovementDirection = Vector3.zero;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Action.performed += BalloonPump;
        _inputs.Player.Action.canceled += StopBalloonFloating;
        _inputs.Player.Move.performed += GetMoveValue;
        _inputs.Player.Move.canceled += GetMoveValue;
        _inputs.Player.SecondaryAction.performed += CancelBalloonFloating;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Action.performed -= BalloonPump;
        _inputs.Player.Action.canceled -= StopBalloonFloating;
        _inputs.Player.Move.performed -= GetMoveValue;
        _inputs.Player.Move.canceled -= GetMoveValue;
        _inputs.Player.SecondaryAction.performed -= CancelBalloonFloating;

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
            Vector3 lateralMoveDirection = (Vector3.Scale(Camera.main.transform.forward, new Vector3(1,0,1)) * _lateralMovementDirection.y + Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * _lateralMovementDirection.x).normalized;
            Vector3 direction = (lateralMoveDirection == Vector3.zero) ? Vector3.up * _verticalDashForce : lateralMoveDirection * _lateralDashForce;
            CharacterControllerTest.Instance.AddForce(direction - _currentDashDirection, _jumpDecel);
            _currentDashDirection = direction;

            Collider collider = CharacterControllerTest.Instance.GetComponent<Collider>();
            Instantiate(_floatFXPrefab, collider.bounds.center - collider.bounds.extents.y * direction.normalized, _floatFXPrefab.transform.rotation);
        }
        _currentAir -= airReductionSpeed * Time.deltaTime;
        if (_currentAir <= 0) ResetBalloonScale();
    }

    private void UpdateUI()
    {
        _airUI.fillAmount = _currentAir / _maxAir;
    }

    private void BalloonPump(InputAction.CallbackContext context)
    {
        if (_isFloatCanceled)
        {
            AfterFloatCancelJump();
        }

        _currentDashDirection = Vector3.zero;
        _isFloating = true;
        _isDashing = true;
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

    private void StopBalloonFloating(InputAction.CallbackContext context)
    {
        _isDashing = false;
        CharacterControllerTest.Instance.AddForce(- _currentDashDirection, _dashDecel);
        _currentDashDirection = Vector3.zero;
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
        _lateralMovementDirection = context.ReadValue<Vector2>();
    }

}
