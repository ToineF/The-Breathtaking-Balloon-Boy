using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFloatingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FLOATING");
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.FloatingGravity, manager.Data.FallData.FloatingMinGravity, manager.Data.FallData.FloatingMaxGravity, manager.Data.FallData.FloatingGravityIncreaseByFrame, manager.Data.FallData.FloatingGravityIncreaseDecelerationByFrame, manager.Data.FallData.FloatingGravityTime, manager.Data.FallData.FloatingGravityAccel);
            
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BaseFloatLateralSpeed, manager.Data.LateralMovementData.BaseFloatTime, manager.Data.LateralMovementData.BaseFloatCurve);

        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.AirManager.ReduceAir(manager.Data.AirData.FloatingAirReductionSpeed);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForBalloonBounce(manager);


            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForPropulsionStartOnAir(manager);

            manager.MovementManager.CheckForJacketDeflated(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.FloatDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}