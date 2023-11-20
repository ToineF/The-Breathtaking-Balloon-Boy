using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBuildingManager : MonoBehaviour
{
    public static CharacterBuildingManager Instance;
    public PlayerInputs _inputs;
    public bool IsActive;

    private BuildingManager _currentBuilding;

    private void Awake()
    {
        Instance = this;
        _inputs = new PlayerInputs();
        IsActive = false;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        _inputs.Player.Move.performed += MoveBuilding;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        _inputs.Player.Move.performed -= MoveBuilding;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out BuildingManager buildingManager)) return;
        buildingManager.IsActivatable = true;
        _currentBuilding = buildingManager;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out BuildingManager buildingManager)) return;
        buildingManager.IsActivatable = false;
        _currentBuilding = null;
    }

    public void StartBuildingManager()
    {
        IsActive = !IsActive;
        GetComponent<CharacterControllerTest>().CanMove = !IsActive;
    }

    private void MoveBuilding(InputAction.CallbackContext context)
    {
        if (!_currentBuilding || !IsActive) return;

        int direction = (int)context.ReadValue<Vector2>().normalized.y;
        _currentBuilding.MoveBuilding(direction);
    }
}
