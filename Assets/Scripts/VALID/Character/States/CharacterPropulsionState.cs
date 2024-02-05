using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterPropulsionState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("PROPULSION");
            manager.MovementManager.LerpGravityTo(manager, manager.MovementManager.FallData.PropulsionGravity, manager.MovementManager.FallData.PropulsionMinGravity, manager.MovementManager.FallData.PropulsionMaxGravity, manager.MovementManager.FallData.PropulsionGravityIncreaseByFrame, manager.MovementManager.FallData.PropulsionGravityTime, manager.MovementManager.FallData.PropulsionGravityAccel);


            //manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.LateralMovementData.BasePropulsionLateralDeplacementSpeed, manager.MovementManager.LateralMovementData.BasePropulsionDeplacementTime, manager.MovementManager.LateralMovementData.BasePropulsionDeplacementCurve);

            manager.MovementManager.LerpPropulsionSpeed(manager, 1, manager.MovementManager.PropulsionData.BasePropulsionAccelTime, manager.MovementManager.PropulsionData.BasePropulsionAccelCurve);
        }

        public override void ExitState(CharacterManager manager)
        {
            manager.MovementManager.LerpPropulsionSpeed(manager, 0, manager.MovementManager.PropulsionData.BasePropulsionDecelTime, manager.MovementManager.PropulsionData.BasePropulsionDecelCurve);
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.AirManager.PropulsionAirReductionSpeed);

            manager.MovementManager.CheckForPropulsionEnd(manager);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForFloatCancel(manager);

            manager.MovementManager.FallfAirEmpty(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.LateralMovementData.PropulsionDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager);

            manager.MovementManager.UpdateGravity(manager);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
