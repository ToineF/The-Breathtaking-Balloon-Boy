using UnityEngine;
using UnityEngine.Windows;

namespace BlownAway.Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("WALK");
        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }
        public override void UpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraPosition();

            if (CharacterManager.Instance.Inputs.MoveInputDirection.magnitude <= 0.0001f)
            {
                manager.SwitchState(manager.IdleState);
                return;
            }

            CharacterManager.Instance.MovementManager.CheckIfGrounded(manager);

            CharacterManager.Instance.MovementManager.CheckForPropulsionStart(manager);

        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MovementManager.MoveAtSpeed(CharacterManager.Instance.MovementManager.BaseWalkSpeed);
        }

        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraAngle();

        }
    }
}