using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    [SerializeField] private int _airFloorHeight = 3;
    [SerializeField] private int _inflationLevel;
    [SerializeField] private float[] _balloonScaleLevel;

    private PlayerInputs _inputs;
    private bool _isActivatable;
    private int _minLevel = 0;
    private int _maxLevel = 3;
    private float _lowerPositionY;

    private void Awake()
    {
        _inputs = new PlayerInputs();
        IsActivatable = false;
        _lowerPositionY = _building.transform.position.y - _inflationLevel * _airFloorHeight;
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
        bool canPlayerMove = !CharacterBuildingManager.Instance.IsActive;
        if (!IsActivatable && canPlayerMove) return;
        CharacterBuildingManager.Instance.StartBuildingManager();
    }

    public void MoveBuilding(int value)
    {
        _inflationLevel = Mathf.Clamp(_inflationLevel + value, _minLevel, _maxLevel);
        _balloon.transform.DOScale(_balloonScaleLevel[_inflationLevel], 1);
        _building.transform.DOMoveY(_lowerPositionY + _inflationLevel * _airFloorHeight, 1);
    }

}
