using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFloatingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FLOATING");
            manager.MovementManager.SetGravityTo(manager.MovementManager.FloatingGravity, manager.MovementManager.FloatingMaxGravity);

        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();

            manager.MovementManager.CheckIfGrounded(manager.States);

            manager.MovementManager.CheckForPropulsionStart(manager.States);
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdateGravity();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle();
        }
    }
}