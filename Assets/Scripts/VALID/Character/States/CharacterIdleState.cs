using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("IDLE");

            // Movements
            manager.Inputs.ResetLastPropulsionInputDirection();
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.IdleData);

            // Gravity
            //manager.MovementManager.LerpGravityTo(manager, manager.Data.GroundDetectionData.GroundedGravity, 0, manager.Data.GroundDetectionData.GroundedGravity, 0, 0, 0.001f, AnimationCurve.Linear(0, 0, 1, 1));

            // Air
            manager.AirManager.RefreshAir();
            manager.MovementManager.RefreshDashes(manager);

            // Float
            manager.MovementManager.StartDeriveTimer(manager);

            manager.MovementManager.ToggleJacketInflate(manager, false);
        }

        public override void ExitState(CharacterManager manager)
        {
            //manager.MovementManager.ResetGravity(manager);
        }

        public override void UpdateState(CharacterManager manager)
        {
            if (manager.Inputs.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.States.SwitchState(manager.States.WalkingState);
                return;
            }

            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForJumpStart(manager);

            //manager.MovementManager.CheckForDashStart(manager, true);

            //manager.MovementManager.CheckForJacketToggle(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.StopMoving(manager);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            //manager.MovementManager.UpdateGravity(manager, false);

            manager.MovementManager.UpdateExternalForces();

            manager.MovementManager.UpdateStickToGround(manager);
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }

}