using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("FALLING");
            CharacterManager.Instance.MovementManager.SetGravityTo(CharacterManager.Instance.MovementManager.BaseGravity, CharacterManager.Instance.MovementManager.BaseMaxGravity);
        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraPosition();

            CharacterManager.Instance.MovementManager.CheckIfGrounded(manager);

            CharacterManager.Instance.MovementManager.CheckForPropulsionStart(manager);
        }


        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MovementManager.MoveAtSpeed(CharacterManager.Instance.MovementManager.FallDeplacementSpeed);

            CharacterManager.Instance.MovementManager.UpdateGravity();
        }

        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraAngle();

        }
    }
}
