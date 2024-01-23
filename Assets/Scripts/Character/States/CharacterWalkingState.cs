using UnityEngine;

namespace Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("WALK");
            manager.InputActions.Player.Move.performed += CharacterManager.Instance.OnMoveInput;
            manager.InputActions.Player.Move.canceled += CharacterManager.Instance.OnMoveInput;
        }

        public override void ExitState(CharacterStatesManager manager)
        {
            manager.InputActions.Player.Move.performed -= CharacterManager.Instance.OnMoveInput;
            manager.InputActions.Player.Move.canceled -= CharacterManager.Instance.OnMoveInput;
        }
        public override void UpdateState(CharacterStatesManager manager)
        {
            if (CharacterManager.Instance.MoveInputDirection.magnitude <= 0.0001f)
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
        }
    }
}