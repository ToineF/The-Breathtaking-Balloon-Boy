using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CharacterController))] [HelpURL("https://antoine-foucault.itch.io/")]
public class CharacterControllerTest: MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorWalkBool;
    private CharacterController _characterController;
    private Vector3 _lastDirection;
    private Vector3 _moveVector;
    [SerializeField] private float Gravity;
    public float Force;
    private float _currentForce;
    [SerializeField] [Range(0,1)] private float _startLerpValue;
    [SerializeField] [Range(0,1)] private float _stopLerpValue;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentForce = Force;
    }

    private void Update()
    {
        float xPosition = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        float zPosition = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        _moveVector = new Vector3(xPosition * Speed, 0, zPosition * Speed) * Time.deltaTime;
        SetAnimation(_moveVector);
        _characterController.Move(_moveVector);

    }

    void FixedUpdate()
    {
        float gravity = Gravity - Force;

        _characterController.Move(Vector3.up * gravity * Time.deltaTime);

        var force = Force;
        var lerp = _currentForce >= 0 ? _stopLerpValue : _startLerpValue;
        Force = Mathf.Lerp(Force, _currentForce, lerp);
        if (Mathf.Abs(force - Force) < 0.001f && _currentForce != 0) _currentForce = 0;
    }

    private void SetAnimation(Vector3 moveVector)
    {
        if (moveVector != Vector3.zero) _lastDirection = moveVector;
        _animator.SetBool(_animatorWalkBool, moveVector != Vector3.zero);
        transform.LookAt(transform.position + moveVector);
    }

    public void SetForce(float value)
    {
        _currentForce = value;
    }
}
