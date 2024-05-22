using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDashState : CharacterBaseState
    {
        public override bool IsMovable { get => true; }

        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("Dash");
            // Movements
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.DashData);
            // Dash
            manager.MovementManager.StartDash(manager);
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdateDashTimer(manager);

            manager.MovementManager.CheckForGroundPound(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            //manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateDashMovement(manager);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();

            manager.MovementManager.CheckIfGrounded(manager, true);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
