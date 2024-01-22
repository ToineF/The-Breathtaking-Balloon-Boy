using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.States
{
    public class CharacterWalkingState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("WALK");
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
            if (CharacterManager.Instance.MoveInputDirection.magnitude <= 0.0001f)
            {
                manager.SwitchState(manager.IdleState);
                return;
            }

        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            
            Vector3 moveDirection = (Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)) * CharacterManager.Instance.MoveInputDirection.z + Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)) * CharacterManager.Instance.MoveInputDirection.x).normalized;
            moveDirection = Vector3.Scale(moveDirection, new Vector3(1, 0, 1));
            //SetAnimation(moveDirection);
            CharacterManager.Instance.Rigidbody.velocity = moveDirection * CharacterManager.Instance.BaseWalkSpeed * Time.deltaTime;
            Debug.Log(moveDirection * CharacterManager.Instance.BaseWalkSpeed * Time.deltaTime);
            //UpdateCamera();
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            float xPosition = context.ReadValue<Vector2>().x;
            float zPosition = context.ReadValue<Vector2>().y;
            CharacterManager.Instance.MoveInputDirection = new Vector3(xPosition, 0, zPosition);
        }
    }
}