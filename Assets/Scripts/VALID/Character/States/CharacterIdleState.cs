using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("IDLE");

            // Movements
            manager.Inputs.ResetLastPropulsionInputDirection();
            manager.MovementManager.LerpDeplacementSpeed(manager, 0, manager.Data.LateralMovementData.BaseIdleTime, manager.Data.LateralMovementData.BaseIdleCurve);

            // Air
            manager.AirManager.RefreshAir();
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            if (manager.Inputs.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.States.SwitchState(manager.States.WalkingState);
                return;
            }

            manager.MovementManager.CheckForPropulsionStartOnGround(manager);

            manager.MovementManager.CheckIfGrounded(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.IdleDirectionTurnSpeed, false);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }

}