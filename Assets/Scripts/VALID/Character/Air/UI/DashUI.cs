using BlownAway.Character.Movements;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class DashUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CharacterMovementManager MovementManager;

        void Update()
        {
            FillAmount = MovementManager.Manager.MovementManager.CurrentDashes > 0 ? 1f : 0f;
        }
    }
}