using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FALLING");
            // Movements
            manager.MovementManager.SetGravityTo(manager, manager.MovementManager.FallData.BaseGravity, manager.MovementManager.FallData.BaseMaxGravity);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.LateralMovementData.BaseFallLateralSpeed, manager.MovementManager.LateralMovementData.BaseFallTime, manager.MovementManager.LateralMovementData.BaseFallCurve);

            // Air
            manager.AirManager.AddAirUntilFullIfEmpty(manager, manager.AirManager.FallingAirRefillSpeed, manager.AirManager.FallingAirRefillDelay);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.AirManager.StopAddingAir();
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);
        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.LateralMovementData.FallDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
