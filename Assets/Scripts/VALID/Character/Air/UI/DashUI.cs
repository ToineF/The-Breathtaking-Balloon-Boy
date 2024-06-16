using BlownAway.Character.Movements;
using System;
using TMPro;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class DashUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CharacterMovementManager MovementManager;

        [Header("Numbers")]
        [SerializeField] private TMP_Text _dashesCountText;
        [SerializeField] private CanvasGroup _dashesCountUI;

        void Update()
        {
            int dashes = MovementManager.Manager.MovementManager.CurrentDashes;
            FillAmount = dashes > 0 ? 1f : 0f;

            UpdateDashCount(dashes);
        }

        private void UpdateDashCount(int dashes)
        {
            _dashesCountUI.alpha = (dashes > 1) ? 1f : 0f;
            _dashesCountText.text = MovementManager.Manager.MovementManager.CurrentDashes.ToString();
        }
    }
}