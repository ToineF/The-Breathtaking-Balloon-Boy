using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterGroundPoundState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("Ground Pound");
            // Movements
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.GroundPoundGravity, manager.Data.FallData.GroundPoundMinGravity, manager.Data.FallData.GroundPoundMaxGravity, manager.Data.FallData.GroundPoundGravityIncreaseByFrame, manager.Data.FallData.GroundPoundGravityIncreaseDecelerationByFrame, manager.Data.FallData.GroundPoundGravityTime, manager.Data.FallData.GroundPoundGravityAccel);
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BaseGroundPoundFallLateralSpeed, manager.Data.LateralMovementData.GroundPoundFallTime, manager.Data.LateralMovementData.GroundPoundFallCurve);

            // Ground Pound
            manager.MovementManager.OnGroundEnter += manager.MovementManager.CheckForGroundPoundStart;
            manager.MovementManager.OnGroundExit += manager.MovementManager.CheckForGroundPoundEnd;
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.CheckIfGrounded(manager);
        }


        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.GroundPoundDirectionTurnSpeed);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
