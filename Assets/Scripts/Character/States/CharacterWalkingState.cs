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
            CharacterManager.Instance.UpdateCameraPosition();

            if (CharacterManager.Instance.Inputs.MoveInputDirection.magnitude <= 0.0001f)
            {
                manager.SwitchState(manager.IdleState);
                return;
            }
            CharacterManager.Instance.CheckIfGrounded(manager);

        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MoveAtSpeed(CharacterManager.Instance.BaseWalkSpeed);
        }

        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.UpdateCameraAngle();

        }
    }
}