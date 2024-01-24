using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterPropulsionState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("PROPULSION");
        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraPosition();

            CharacterManager.Instance.MovementManager.CheckIfGrounded(manager);

            CharacterManager.Instance.MovementManager.CheckForPropulsionEnd(manager);
        }

        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MovementManager.UpdatePropulsionMovement();

            CharacterManager.Instance.MovementManager.UpdateGravity();
        }
        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraAngle();
        }
    }
}
