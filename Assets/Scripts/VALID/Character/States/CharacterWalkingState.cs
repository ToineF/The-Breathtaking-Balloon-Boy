using UnityEngine;
using UnityEngine.Windows;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("WALK");
            manager.MovementManager.LerpDeplacementSpeed(manager, manager.MovementManager.GroundData.BaseWalkSpeed, manager.MovementManager.GroundData.BaseWalkTime, manager.MovementManager.GroundData.BaseWalkCurve);
        }

        public override void ExitState(CharacterManager manager)
        {
        }
        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();

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
            manager.MovementManager.MoveAtSpeed(manager, manager.MovementManager.GroundData.BaseWalkSpeed);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle(manager);

        }
    }
}