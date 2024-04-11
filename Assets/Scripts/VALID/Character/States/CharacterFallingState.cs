using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("FALLING");
            // Movements
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.BaseData);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.FallingData);

            // Air
            manager.AirManager.AddAirUntilFullIfEmpty(manager, manager.Data.AirData.FallingAirRefillSpeed, manager.Data.AirData.FallingAirRefillDelay);

            // Animation
            manager.AnimationManager.PlayAnimation(manager.AnimationManager.FallAnim);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.AirManager.StopAddingAir();
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForJacketToggle(manager);
            manager.MovementManager.CheckForJacketInflated(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForDashStart(manager);

            manager.MovementManager.CheckIfGrounded(manager);

        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
