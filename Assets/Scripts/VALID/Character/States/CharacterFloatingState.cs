using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFloatingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FLOATING");
            manager.MovementManager.LerpGravityTo(manager, manager.MovementManager.FallData.FloatingGravity, manager.MovementManager.FallData.FloatingMinGravity, manager.MovementManager.FallData.FloatingMaxGravity, manager.MovementManager.FallData.FloatingGravityIncreaseByFrame, manager.MovementManager.FallData.FloatingGravityIncreaseDecelerationByFrame, manager.MovementManager.FallData.FloatingGravityTime, manager.MovementManager.FallData.FloatingGravityAccel);
            
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.LateralMovementData.BaseFallLateralSpeed, manager.MovementManager.LateralMovementData.BaseFallTime, manager.MovementManager.LateralMovementData.BaseFallCurve);

        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.AirManager.AirData.FloatingAirReductionSpeed);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForFloatCancel(manager);

            manager.MovementManager.FallfAirEmpty(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.LateralMovementData.FloatDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}