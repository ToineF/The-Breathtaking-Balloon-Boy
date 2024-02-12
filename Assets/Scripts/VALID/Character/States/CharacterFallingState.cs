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
            manager.MovementManager.LerpGravityTo(manager, manager.MovementManager.FallData.BaseGravity, manager.MovementManager.FallData.BaseMinGravity, manager.MovementManager.FallData.BaseMaxGravity, manager.MovementManager.FallData.BaseGravityIncreaseByFrame, manager.MovementManager.FallData.BaseGravityIncreaseDecelerationByFrame, manager.MovementManager.FallData.BaseGravityTime, manager.MovementManager.FallData.BaseGravityAccel);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.LateralMovementData.BaseFallLateralSpeed, manager.MovementManager.LateralMovementData.BaseFallTime, manager.MovementManager.LateralMovementData.BaseFallCurve);

            // Air
            manager.AirManager.AddAirUntilFullIfEmpty(manager, manager.AirManager.AirData.FallingAirRefillSpeed, manager.AirManager.AirData.FallingAirRefillDelay);
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
