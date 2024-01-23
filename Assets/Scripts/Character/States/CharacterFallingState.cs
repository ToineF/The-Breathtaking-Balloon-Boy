using System;
using UnityEngine;

namespace Character.States
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
            CharacterManager.Instance.CheckIfGrounded(manager);
            SetGravity();
        }


        public override void FixedUpdateState(CharacterStatesManager manager)
        {
        }

        public override void LateUpdateState(CharacterStatesManager manager)
        {
        }

        private void SetGravity()
        {
            if (!CharacterManager.Instance.IsGrounded)
            {
                CharacterManager.Instance.CurrentGravity = Mathf.Clamp(CharacterManager.Instance.CurrentGravity + CharacterManager.Instance.GravityIncreaseByFrame, CharacterManager.Instance.BaseGravity, CharacterManager.Instance.MaxGravity);
            }

            /*Vector3 additionalForces = Vector3.zero;
            foreach (var force in _additionnalForces)
            {
                additionalForces += force.Value.CurrentForce;
                force.Value.CurrentForce = Vector3.Lerp(force.Value.CurrentForce, force.Value.TargetForce, force.Value.ForceLerp);
            }*/
            Vector3 gravity = -CharacterManager.Instance.CurrentGravity * new Vector3(0, 1, 0);
            //Vector3 allForces = CharacterManager.Instance + additionalForces + gravity;

            //_characterController.Move(allForces * Time.deltaTime);

            CharacterManager.Instance.CharacterRigidbody.velocity = gravity * Time.deltaTime;
            //CharacterManager.Instance.Force = Vector3.Lerp(CharacterManager.Instance.Force, CharacterManager.Instance.CurrentGravity, _lerpValue);
        }

    }
}
