using BlownAway.Character;
using UnityEngine;
using UnityEngine.UI;

namespace BlownAway.Tutorial
{
    [RequireComponent(typeof(Image))]
    public class TutorialImage : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private CharacterManager _manager;

        [Header("UI")]
        [SerializeField] private Sprite _keyboardSprite;
        [SerializeField] private Sprite _xboxSprite;
        [SerializeField] private Sprite _playstationSprite;
        [SerializeField] private Sprite _switchSprite;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            if (_manager != null)
                _manager.Inputs.OnControllerTypeChange += OnControllerChange;
            OnControllerChange(_manager.Inputs.ControllerType);
        }

        private void OnControllerChange(Character.Inputs.ControllerType type)
        {
            switch (type)
            {
                case Character.Inputs.ControllerType.KEYBOARD:
                    _image.sprite = _keyboardSprite;
                    break;
                case Character.Inputs.ControllerType.XBOX:
                    _image.sprite = _xboxSprite;
                    break;
                case Character.Inputs.ControllerType.PLAYSTATION:
                    _image.sprite = _playstationSprite;
                    break;
                case Character.Inputs.ControllerType.SWITCH:
                    _image.sprite = _switchSprite;
                    break;
            }
        }
    }
}