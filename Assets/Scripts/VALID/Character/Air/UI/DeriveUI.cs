using BlownAway.Character.Movements;
using UnityEngine;

namespace BlownAway.Character.Air
{
    public class DeriveUI : RadialUI
    {
        [Header("Reference")]
        [SerializeField] private CharacterMovementManager MovementManager;

        void Update()
        {
            FillAmount = MovementManager.Manager.MovementManager.NormalizedDeriveAirAmount;
        }
    }
}