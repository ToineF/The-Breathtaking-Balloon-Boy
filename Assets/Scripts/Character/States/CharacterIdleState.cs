using UnityEngine;
using Character;
using UnityEngine.InputSystem;


namespace Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("IDLE");
            manager.InputActions.Player.Move.performed += OnMoveInput;
            manager.InputActions.Player.Move.canceled += OnMoveInput;
        }

        public override void ExitState(CharacterStatesManager manager)
        {
            manager.InputActions.Player.Move.performed -= OnMoveInput;
            manager.InputActions.Player.Move.canceled -= OnMoveInput;
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
            if (CharacterManager.Instance.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.SwitchState(manager.WalkingState);
                return;
            }
        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.Rigidbody.velocity = Vector3.zero;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            CharacterManager.Instance.MoveInputDirection = new Vector3(xPosition, 0, zPosition);
        }
    }

}