using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("IDLE");
        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraPosition();
            if (CharacterManager.Instance.Inputs.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.SwitchState(manager.WalkingState);
                return;
            }

            CharacterManager.Instance.MovementManager.CheckIfGrounded(manager);

            CharacterManager.Instance.MovementManager.CheckForPropulsionStart(manager);

        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CharacterRigidbody.velocity = Vector3.zero;
        }
        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraAngle();
        }
    }

}