using System;
using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterFallingState : CharacterBaseState
    {
        public override void EnterState(CharacterStatesManager manager)
        {
            Debug.Log("FALLING");
        }

        public override void ExitState(CharacterStatesManager manager)
        {
        }

        public override void UpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraPosition();

            CharacterManager.Instance.MovementManager.CheckIfGrounded(manager);
        }


        public override void FixedUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.MovementManager.MoveAtSpeed(CharacterManager.Instance.MovementManager.FallDeplacementSpeed);
            SetGravity();
        }

        public override void LateUpdateState(CharacterStatesManager manager)
        {
            CharacterManager.Instance.CameraManager.UpdateCameraAngle();

        }

        private void SetGravity()
        {
            if (!CharacterManager.Instance.MovementManager.IsGrounded)
            {
                CharacterManager.Instance.MovementManager.CurrentGravity = Mathf.Clamp(CharacterManager.Instance.MovementManager.CurrentGravity + CharacterManager.Instance.MovementManager.GravityIncreaseByFrame, CharacterManager.Instance.MovementManager.BaseGravity, CharacterManager.Instance.MovementManager.MaxGravity);
            }

            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -CharacterManager.Instance.MovementManager.CurrentGravity * Vector3.up;
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            CharacterManager.Instance.MovementManager.CurrentVelocity += gravity * Time.deltaTime;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

    }
}
