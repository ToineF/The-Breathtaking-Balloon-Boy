using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[HelpURL("https://antoine-foucault.itch.io/")]
public class CharacterControllerTest : MonoBehaviour
{
    private PlayerInputs _inputs;

    [SerializeField] private float Speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorWalkBool;
    private CharacterController _characterController;
    private Vector3 _lastDirection;
    private Vector3 _moveVector = Vector3.zero;
    [SerializeField] private float Gravity;
    public Vector3 Force;
    private Vector3 _currentForce;
    private float _lerpValue;

    private void Awake()
    {
        _inputs = new PlayerInputs();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentForce = Force;
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
        Vector3 gravity = new Vector3(Force.x, Force.y - Gravity, Force.z);

        _characterController.Move(gravity * Time.deltaTime);

        var forceSign = Mathf.Sign(_currentForce.x) * Mathf.Sign(_currentForce.x) * Mathf.Sign(_currentForce.x);
        Force = Vector3.Lerp(Force, _currentForce, _lerpValue);
    }

    private void SetAnimation(Vector3 moveVector)
    {
        if (moveVector != Vector3.zero) _lastDirection = moveVector;
        _animator.SetBool(_animatorWalkBool, moveVector != Vector3.zero);
        transform.LookAt(transform.position + moveVector);
    }

    public void SetForce(Vector3 force, float value)
    {
        _currentForce = force;
        _lerpValue = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
    }
}
