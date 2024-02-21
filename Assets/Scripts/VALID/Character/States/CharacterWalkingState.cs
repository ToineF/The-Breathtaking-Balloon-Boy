using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("WALK");
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.Data.LateralMovementData.BaseWalkSpeed, manager.Data.LateralMovementData.BaseWalkTime, manager.Data.LateralMovementData.BaseWalkCurve);

            // Gravity
            //manager.MovementManager.LerpGravityTo(manager, manager.Data.GroundDetectionData.GroundedGravity, 0, manager.Data.GroundDetectionData.GroundedGravity, 0, 0, 0.001f, AnimationCurve.Linear(0, 0, 1, 1));
        }

        public override void ExitState(CharacterManager manager)
        {
            //manager.MovementManager.ResetGravity(manager);
        }
        public override void UpdateState(CharacterManager manager)
        {

            if (manager.Inputs.MoveInputDirection.magnitude <= 0.0001f)
            {
                manager.States.SwitchState(manager.States.IdleState);
                return;
            }

            manager.MovementManager.CheckIfGrounded(manager);

            manager.MovementManager.CheckForPropulsionStartOnGround(manager);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager, manager.Data.LateralMovementData.WalkDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);

            manager.MovementManager.UpdateGravity(manager, false);

        }

        public override void LateUpdateState(CharacterManager manager)
        {

        }
    }
}