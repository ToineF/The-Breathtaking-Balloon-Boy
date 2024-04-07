using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("DEATH");
            manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
            manager.MovementManager.ResetAllExternalForces();
            manager.CameraManager.IsMovable = false;
            manager.States.SwitchState(manager.States.FallingState);
            //manager.Transition.SetTransition(() =>
            //{
            //    manager.Transition.PlayFadeIn();
            //    Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //    manager.States.SwitchState(manager.States.FallingState);

            //    Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //    Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //});

            //Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
        }

        public override void ExitState(CharacterManager manager)
        {
            Debug.Log("death end");
            //manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
            manager.CameraManager.IsMovable = true;
        }

        public override void UpdateState(CharacterManager manager)
        {
            Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
            manager.MovementManager.UpdateGravity(manager);
        }

        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
