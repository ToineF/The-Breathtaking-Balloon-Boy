using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] [HelpURL("https://antoine-foucault.itch.io/")]
public class CharacterControllerTest: MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorWalkBool;
    private CharacterController _characterController;
    private Vector3 _lastDirection;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float xPosition = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        float zPosition = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        Vector3 moveVector = new Vector3(xPosition, 0, zPosition) * Speed;
        SetAnimation(moveVector);
        _characterController.Move(moveVector);
    }

    private void SetAnimation(Vector3 moveVector)
    {
        if (moveVector != Vector3.zero) _lastDirection = moveVector;
        _animator.SetBool(_animatorWalkBool, moveVector != Vector3.zero);
        transform.LookAt(transform.position + moveVector);
    }
}
