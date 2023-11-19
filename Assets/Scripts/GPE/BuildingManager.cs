using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collision))]
public class BuildingManager : MonoBehaviour
{
    public bool IsActivatable { get => _isActivatable; set {
            _isActivatable = value;
            _UIIsActivatable.SetActive(value);
        }}

    [Header("References")]
    [SerializeField] private GameObject _building;
    [SerializeField] private GameObject _balloon;
    [SerializeField] private GameObject _UIIsActivatable;

    [Header("Properties")]
    [SerializeField] private int _inflationLevel;
    [SerializeField] private float[] _balloonScaleLevel;

    private bool _isActivatable;
    private PlayerInputs _inputs;

    private void Awake()
    {
        _inputs = new PlayerInputs();
        IsActivatable = false;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Action.performed += StartBuildingMoveState;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Action.performed -= StartBuildingMoveState;
    }

    private void StartBuildingMoveState(InputAction.CallbackContext context)
    {
        if (!IsActivatable) return;
        Debug.Log("hey");
    }

}
