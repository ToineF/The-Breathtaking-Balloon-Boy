using BlownAway.Character;
using UnityEngine;
using UnityEngine.UI;

namespace BlownAway.Tutorial {
    public class TutorialSign : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private CharacterManager _manager;
        [SerializeField] private Image _image;

        [Header("UI")]
        [SerializeField] private Sprite _keyboardSprite;
        [SerializeField] private Sprite _controllerSprite;

        private void Update()
        {
            _image.sprite = _manager.Inputs.IsMouse ? _keyboardSprite : _controllerSprite;
        }
    }
}