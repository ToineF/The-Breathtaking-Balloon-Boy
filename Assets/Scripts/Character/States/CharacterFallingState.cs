using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("FALLING");
            manager.MovementManager.SetGravityTo(manager.MovementManager.BaseGravity, manager.MovementManager.BaseMaxGravity);
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
            manager.MovementManager.MoveAtSpeed(manager.MovementManager.FallDeplacementSpeed);

            manager.MovementManager.UpdateGravity();
        }

        public override void LateUpdateState(CharacterManager manager)
        {
            manager.CameraManager.UpdateCameraAngle();

        }
    }
}
