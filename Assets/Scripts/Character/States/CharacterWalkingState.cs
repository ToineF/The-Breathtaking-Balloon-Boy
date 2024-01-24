using UnityEngine;
using UnityEngine.Windows;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("WALK");
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

            manager.MovementManager.CheckIfGrounded(manager.States);

            manager.MovementManager.CheckForPropulsionStart(manager.States);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.MoveAtSpeed(manager.MovementManager.BaseWalkSpeed);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle();

        }
    }
}