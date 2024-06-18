using UnityEngine;

namespace BlownAway.Cutscenes
    {
    public class CutsceneSkipUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CutsceneManager _cutsceneManager;

        private void Update()
        {
            FillAmount = _cutsceneManager.NormalizedSkipCutsceneTime;
        }
    }
}