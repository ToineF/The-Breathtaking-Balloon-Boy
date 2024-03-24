using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDashState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
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

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForBalloonBounce(manager);

            manager.MovementManager.CheckForGroundPound(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            //manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateDashMovement(manager);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
