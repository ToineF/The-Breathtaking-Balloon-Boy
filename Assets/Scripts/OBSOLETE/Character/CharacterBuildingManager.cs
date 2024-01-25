using BlownAway.GPE.Buildings;
using BlownAway.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlownAway.Player
{
    public class CharacterBuildingManager : MonoBehaviour
    {
        public static CharacterBuildingManager Instance;
        public PlayerInputs _inputs;
        public bool IsActive;
        public BuildingManager CurrentBuilding;


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
            CurrentBuilding = buildingManager;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out BuildingManager buildingManager)) return;
            buildingManager.IsActivatable = false;
            CurrentBuilding = null;
        }

        public void StartBuildingManager()
        {
            IsActive = !IsActive;
            GetComponent<CharacterControllerTest>().CanMove = !IsActive;
        }

        private void MoveBuilding(InputAction.CallbackContext context)
        {
            if (!CurrentBuilding || !IsActive) return;

            int direction = (int)context.ReadValue<Vector2>().normalized.y;
            if (direction == 0) return;

            CurrentBuilding.MovePreviewBuilding(direction);
        }
    }
}
