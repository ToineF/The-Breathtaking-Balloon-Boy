using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterPropulsionState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("PROPULSION");
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.PropulsionGravity, manager.Data.FallData.PropulsionMinGravity, manager.Data.FallData.PropulsionMaxGravity, manager.Data.FallData.PropulsionGravityIncreaseByFrame, manager.Data.FallData.PropulsionGravityIncreaseDecelerationByFrame, manager.Data.FallData.PropulsionGravityTime, manager.Data.FallData.PropulsionGravityAccel);

            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BasePropulsionLateralDeplacementSpeed, manager.Data.LateralMovementData.BasePropulsionDeplacementTime, manager.Data.LateralMovementData.BasePropulsionDeplacementCurve);

            manager.MovementManager.LerpPropulsionSpeed(manager, 1, manager.Data.PropulsionData.BasePropulsionAccelTime, manager.Data.PropulsionData.BasePropulsionAccelCurve);

            manager.MovementManager.StartPropulsionTimer(manager);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.MovementManager.LerpPropulsionSpeed(manager, 0, manager.Data.PropulsionData.BasePropulsionDecelTime, manager.Data.PropulsionData.BasePropulsionDecelCurve);
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.Data.AirData.PropulsionAirReductionSpeed);

            manager.MovementManager.FallIfAirEmpty(manager);

            manager.MovementManager.UpdatePropulsionTimer(manager);

            if (manager.MovementManager.PropulsionTimer > 0) return;

            manager.MovementManager.CheckForPropulsionEnd(manager);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForBalloonBounce(manager);

            manager.MovementManager.CheckForGroundPound(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            if (!manager.Inputs.PropulsionType.HasFlag(Inputs.PropulsionDirection.Lateral))
                manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.PropulsionDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
