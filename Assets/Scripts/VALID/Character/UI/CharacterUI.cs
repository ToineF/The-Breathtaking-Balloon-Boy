using UnityEngine;

namespace BlownAway.Character
{
    public class CharacterUI : CharacterSubComponent
    {
        [SerializeField] private CanvasGroup UI;

        private bool _isHidden = false;
        public void ToggleUI()
        {
            _isHidden = !_isHidden;
            UpdateUI();
        }

        public void ShowUI(bool visible)
        {
            _isHidden = !visible;
            UpdateUI();
        }

        private void UpdateUI()
        {
            int alpha = _isHidden ? 0 : 1;
            UI.alpha = alpha;
        }
    }
}