using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] [HelpURL("https://antoine-foucault.itch.io/")]
public class CharacterControllerTest: MonoBehaviour
{

    [SerializeField] private float Speed;
    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed;
        _characterController.Move(moveVector);
    }
}
