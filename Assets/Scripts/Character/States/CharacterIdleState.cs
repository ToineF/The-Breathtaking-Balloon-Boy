using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("IDLE");
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
            CharacterManager.Instance.UpdateCamera();
            if (CharacterManager.Instance.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.SwitchState(manager.WalkingState);
                return;
            }
            CharacterManager.Instance.CheckIfGrounded(manager);
        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CharacterRigidbody.velocity = Vector3.zero;
        }
        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MoveCamera();
        }
    }

}