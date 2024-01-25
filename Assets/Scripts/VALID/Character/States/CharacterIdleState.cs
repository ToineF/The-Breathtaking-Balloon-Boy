using UnityEngine;


namespace BlownAway.Character.States
{
    public class CharacterIdleState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("IDLE");
            manager.Inputs.ResetLastMoveInputDirection();
            manager.AirManager.RefreshAir();

        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();
            if (manager.Inputs.MoveInputDirection.magnitude > 0.0001f)
            {
                manager.States.SwitchState(manager.States.WalkingState);
                return;
            }

            manager.MovementManager.CheckForPropulsionStartOnGround(manager);

            manager.MovementManager.CheckIfGrounded(manager);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.CharacterRigidbody.velocity = Vector3.zero;
        }
        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle(manager);
        }
    }

}