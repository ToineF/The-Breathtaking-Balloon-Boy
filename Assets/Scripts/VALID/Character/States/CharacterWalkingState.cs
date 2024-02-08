using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("WALK");
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.LateralMovementData.BaseWalkSpeed, manager.MovementManager.LateralMovementData.BaseWalkTime, manager.MovementManager.LateralMovementData.BaseWalkCurve);
        }

        public override void ExitState(CharacterManager manager)
        {
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
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.LateralMovementData.WalkDirectionTurnSpeed);

            manager.MovementManager.UpdatePropulsionMovement(manager, false);
        }

        public override void LateUpdateState(CharacterManager manager)
        {

        }
    }
}