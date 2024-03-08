using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeriveState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("DERIVE");
            manager.MovementManager.LerpGravityTo(manager, manager.Data.FallData.DeriveGravity, manager.Data.FallData.DeriveMinGravity, manager.Data.FallData.DeriveMaxGravity, manager.Data.FallData.DeriveGravityIncreaseByFrame, manager.Data.FallData.DeriveGravityIncreaseDecelerationByFrame, manager.Data.FallData.DeriveGravityTime, manager.Data.FallData.DeriveGravityAccel);

            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BaseDeriveLateralSpeed, manager.Data.LateralMovementData.BaseDeriveTime, manager.Data.LateralMovementData.BaseDeriveCurve);
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdateDeriveTimer(manager);

            manager.MovementManager.CheckForDeriveEnd(manager);

            manager.MovementManager.CheckIfGrounded(manager, true);

            manager.MovementManager.CheckForBalloonBounce(manager);

            manager.MovementManager.CheckForGroundPound(manager);

            manager.MovementManager.CheckForFloatCancel(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.DeriveDirectionTurnSpeed);

            manager.MovementManager.UpdateGravity(manager);

            manager.MovementManager.UpdateExternalForces();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
