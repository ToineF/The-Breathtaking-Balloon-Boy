using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[HelpURL("https://antoine-foucault.itch.io/")]
public class CharacterControllerTest : MonoBehaviour
{
    public Action OnGroundEnter;

    [Header("Walk")]
    [SerializeField] private float Speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorWalkBool;

    [Header("Gravity")]
    [SerializeField] private float BaseGravity;
    [SerializeField] private float MaxGravity;
    [SerializeField] private float _gravityIncreaseByFrame;
    private float _currentGravity;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _sphereRadius;
    [SerializeField] private LayerMask _playerLayer;
    public bool IsGrounded;

    // References
    private PlayerInputs _inputs;
    private CharacterController _characterController;

    // Walk
    private Vector3 _moveVector = Vector3.zero;
    public Vector3 Force;
    private Vector3 _currentForce;
    private float _lerpValue;

    // Ground Check
    private RaycastHit[] _groundHitResults;

    private void Awake()
    {
        _inputs = new PlayerInputs();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentForce = Force;
        _groundHitResults = new RaycastHit[2];
        _currentGravity = BaseGravity;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Move.performed += StartMove;
        _inputs.Player.Move.canceled += StopMove;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Move.performed -= StartMove;
        _inputs.Player.Move.canceled -= StopMove;

    }

    private void Update()
    {
        SetAnimation(_moveVector);
        _characterController.Move(_moveVector);

        var grounded = IsGrounded;
        IsGrounded = Physics.SphereCastNonAlloc(transform.position, _sphereRadius, Vector3.down, _groundHitResults, _groundCheckDistance, _playerLayer) > 0;
        if (grounded != IsGrounded && IsGrounded)
        {
            OnGroundEnter?.Invoke();
            _currentGravity = BaseGravity;
        }
    }

    private void StartMove(InputAction.CallbackContext context)
    {
        float xPosition = context.ReadValue<Vector2>().x;
        float zPosition = context.ReadValue<Vector2>().y;
        _moveVector = new Vector3(xPosition * Speed, 0, zPosition * Speed) * Time.deltaTime;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _moveVector = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!IsGrounded)
        {
            _currentGravity = Mathf.Clamp(_currentGravity + _gravityIncreaseByFrame, BaseGravity, MaxGravity);
        }

        Vector3 gravity = new Vector3(Force.x, Force.y - _currentGravity, Force.z);

        _characterController.Move(gravity * Time.deltaTime);

        var forceSign = Mathf.Sign(_currentForce.x) * Mathf.Sign(_currentForce.x) * Mathf.Sign(_currentForce.x);
        Force = Vector3.Lerp(Force, _currentForce, _lerpValue);
    }

    private void SetAnimation(Vector3 moveVector)
    {
        _animator.SetBool(_animatorWalkBool, moveVector != Vector3.zero);
        transform.LookAt(transform.position + moveVector);
    }

    public void SetForce(Vector3 force, float value)
    {
        _currentForce = force;
        _lerpValue = value;
    }

    public void SetForceForTime(Vector3 force, float time, float startLerp, float endLerp)
    {
        SetForce(force, startLerp);
        StartCoroutine(WaitForAction(time, () => SetForce(_currentForce-force, endLerp)));
    }

    private IEnumerator WaitForAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
