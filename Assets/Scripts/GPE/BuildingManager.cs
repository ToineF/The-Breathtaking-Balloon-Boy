using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using BlownAway.Player;

namespace BlownAway.GPE.Buildings {

    [RequireComponent(typeof(Collision))]
    public class BuildingManager : MonoBehaviour
    {
        public bool IsActivatable
        {
            get => _isActivatable; set
            {
                _isActivatable = value;
                _UIIsActivatable.SetActive(value);
            }
        }

        [Header("References")]
        [SerializeField] private Building _building;
        [SerializeField] private GameObject _balloon;
        [SerializeField] private GameObject _UIIsActivatable;

        [Header("Properties")]
        [SerializeField] private float _buildingMoveSpeed = 1;
        [SerializeField] private int _airFloorHeight = 3;
        [SerializeField] private int _inflationLevel;
        [SerializeField] private float[] _balloonScaleLevel;
        [SerializeField] private GameObject _UIAirLevel;
        [SerializeField] private GameObject _UIAirLevelArrow;
        [SerializeField] private float _UIArrowincreaseHeight = 50;
        [SerializeField] private float _referenceScreenHeight = 1080;

        private PlayerInputs _inputs;
        private bool _isActivatable;
        private int _minLevel = 0;
        private int _maxLevel = 3;
        private float _lowerPositionY;
        private float _startUIY;
        private Vector3 _ballonBaseScale;
        private GameObject _ghostBuilding;


        private void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void Start()
        {
            IsActivatable = false;
            _lowerPositionY = _building.transform.position.y - _inflationLevel * _airFloorHeight;
            _startUIY = _UIAirLevelArrow.transform.position.y;
            _UIAirLevelArrow.transform.DOMoveY(_UIAirLevel.transform.position.y + _inflationLevel * _UIArrowincreaseHeight, 0);
            _UIAirLevel.SetActive(false);
            _UIAirLevelArrow.transform.DOMoveY(_startUIY + _inflationLevel * _UIArrowincreaseHeight * (Screen.height / _referenceScreenHeight), 1);
            _ballonBaseScale = _balloon.transform.localScale;
            _ghostBuilding = _building.GhostBuilding;
        }

        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Player.Action.performed += CheckCanMoveBuilding;
            _inputs.Player.Cancel.performed += CheckCanMoveBuilding;
        }

        private void OnDisable()
        {
            _inputs.Disable();
            _inputs.Player.Action.performed -= CheckCanMoveBuilding;
            _inputs.Player.Cancel.performed -= CheckCanMoveBuilding;
        }

        private void CheckCanMoveBuilding(InputAction.CallbackContext context)
        {
            bool canPlayerMove = CharacterBuildingManager.Instance.IsActive;
            bool isCurrentBuilding = CharacterBuildingManager.Instance.CurrentBuilding == this || CharacterBuildingManager.Instance.CurrentBuilding == null;
            if (!IsActivatable || !isCurrentBuilding) return;

            CharacterBuildingManager.Instance.StartBuildingManager();

            _UIAirLevel.SetActive(!canPlayerMove);
            _ghostBuilding.SetActive(!canPlayerMove);
            _ghostBuilding.transform.position = new Vector3(_ghostBuilding.transform.position.x, _lowerPositionY + _inflationLevel * _airFloorHeight, _ghostBuilding.transform.position.z);
            if (canPlayerMove) MoveBuilding();
        }

        public void MovePreviewBuilding(int value)
        {
            _inflationLevel = Mathf.Clamp(_inflationLevel + value, _minLevel, _maxLevel);
            _balloon.transform.DOScale(_ballonBaseScale * _balloonScaleLevel[_inflationLevel], _buildingMoveSpeed);
            _UIAirLevelArrow.transform.DOMoveY(_startUIY + _inflationLevel * _UIArrowincreaseHeight * (Screen.height / _referenceScreenHeight), _buildingMoveSpeed);

            _ghostBuilding.transform.DOKill();
            _ghostBuilding.transform.DOMoveY(_lowerPositionY + _inflationLevel * _airFloorHeight, _buildingMoveSpeed);

        }

        private void MoveBuilding()
        {
            _building.transform.DOMoveY(_lowerPositionY + _inflationLevel * _airFloorHeight, _buildingMoveSpeed);
        }
    }
}
