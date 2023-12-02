using BlownAway.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

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
    [SerializeField] private float _dashForce;

    [Header("Visuals")]
    [SerializeField] private GameObject _jumpFXPrefab;
    [SerializeField] private GameObject _balloonVisual;
    [SerializeField] private float _balloonScaleValue = 1f;
    [SerializeField] private float _balloonScaleTime = 1f;

    private int _jumps;
    private float _currentAir;
    private bool _isFloating;
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
        _currentAir = 0;
        _balloonOriginalScale = _balloonVisual.transform.localScale;
        _currentDashDirection = Vector3.zero;
        _lateralMovementDirection = Vector3.zero;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Action.performed += BalloonBump;
        _inputs.Player.Action.canceled += StopBalloonFloating;
        _inputs.Player.Move.performed += GetMoveValue;
        _inputs.Player.Move.canceled += GetMoveValue;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Action.performed -= BalloonBump;
        _inputs.Player.Action.canceled -= StopBalloonFloating;
        _inputs.Player.Move.performed -= GetMoveValue;
        _inputs.Player.Move.canceled -= GetMoveValue;

    }

    private void Update()
    {
        if (_currentAir <= 0) return;

        float airReductionSpeed = _airReductionSpeed;
        if (_isFloating)
        {
            airReductionSpeed += _airDashReductionSpeed;
            Vector3 lateralMoveDirection = (Camera.main.transform.forward * _lateralMovementDirection.z + Camera.main.transform.right * _lateralMovementDirection.x).normalized;
            Vector3 direction = (lateralMoveDirection == Vector3.zero) ? Vector3.up : lateralMoveDirection;
            Vector3 hit = direction * _dashForce;
            CharacterControllerTest.Instance.AddForce(hit - _currentDashDirection, _jumpDecel);
            _currentDashDirection = hit;
        }
        _currentAir -= airReductionSpeed * Time.deltaTime;
        if (_currentAir <= 0) ResetBalloonScale();
    }

    private void BalloonBump(InputAction.CallbackContext context)
    {
        _currentDashDirection = Vector3.zero;
        _isFloating = true;

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

        _balloonVisual.transform.DOScale(Vector3.one * _balloonScaleValue, _balloonScaleTime);

        // Air
        _currentAir = _maxAir;

        CharacterControllerTest.Instance.SetFloatingCamera(true);
    }

    private void PlayerEnterGround()
    {
        _jumps = _maxJumps;
        CharacterControllerTest.Instance.OnGroundEnter -= PlayerEnterGround;
        ResetBalloonScale();

    }

    private void ResetBalloonScale()
    {
        CharacterControllerTest.Instance.SetForce(Vector3.zero, 1);
        _balloonVisual.transform.DOScale(_balloonOriginalScale, _balloonScaleTime);
        _currentDashDirection = Vector3.zero;
        CharacterControllerTest.Instance.SetFloatingCamera(false);

    }

    private void StopBalloonFloating(InputAction.CallbackContext context)
    {
        _isFloating = false;
        CharacterControllerTest.Instance.AddForce(- _currentDashDirection, _jumpDecel);
        _currentDashDirection = Vector3.zero;
    }
    private void GetMoveValue(InputAction.CallbackContext context)
    {
        _lateralMovementDirection = context.ReadValue<Vector2>();
    }

}
