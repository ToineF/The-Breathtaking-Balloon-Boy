using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterPropulsionState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("PROPULSION");
        }

        public override void ExitState(CharacterManager manager)
        {
        }

        public override void UpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraPosition();

            manager.MovementManager.CheckForPropulsionEnd(manager.States);

            manager.MovementManager.CheckIfGrounded(manager.States);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdatePropulsionMovement();

            manager.MovementManager.UpdateGravity();
        }
        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle();
        }
    }
}
