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
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.BaseGravity, manager.Data.FallData.BaseMinGravity, manager.Data.FallData.BaseMaxGravity, manager.Data.FallData.BaseGravityIncreaseByFrame, manager.Data.FallData.BaseGravityIncreaseDecelerationByFrame, manager.Data.FallData.BaseGravityTime, manager.Data.FallData.BaseGravityAccel);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BaseFallLateralSpeed, manager.Data.LateralMovementData.BaseFallTime, manager.Data.LateralMovementData.BaseFallCurve);

            // Air
            manager.AirManager.AddAirUntilFullIfEmpty(manager, manager.Data.AirData.FallingAirRefillSpeed, manager.Data.AirData.FallingAirRefillDelay);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.AirManager.StopAddingAir();
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForBalloonBounce(manager);

            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForDeriveStart(manager);
        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.FallDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
