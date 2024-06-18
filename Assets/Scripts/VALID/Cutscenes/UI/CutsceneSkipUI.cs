using UnityEngine;
using UnityEngine.UI;

namespace BlownAway.Cutscenes
    {
    public class CutsceneSkipUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CutsceneManager _cutsceneManager;
        [SerializeField] private Image _background;

        private void Update()
        {
            FillAmount = _cutsceneManager.NormalizedSkipCutsceneTime;
            _background.color = new Color(1,1,1, FillAmount > 0.01f ? 1 : 0);
        }
    }
}