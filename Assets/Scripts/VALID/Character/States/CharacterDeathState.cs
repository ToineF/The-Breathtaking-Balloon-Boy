using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("DEATH");
            manager.MovementManager.ResetAllExternalForces();
            manager.CameraManager.IsMovable = false;
            manager.MovementManager.OnDeath?.Invoke(manager);
            //manager.States.SwitchState(manager.States.FallingState);
            //manager.Transition.SetTransition(() =>
            //{
            //    //manager.Transition.PlayFadeIn();
            //    //Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //    manager.CameraManager.IsMovable = true;

            //    manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
            //    //manager.States.SwitchState(manager.States.FallingState);
            //    manager.Transition.PlayFadeIn();


            //    //Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //    //Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //});

            //Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            manager.States.SwitchState(manager.States.FallingState);

        }

        public override void ExitState(CharacterManager manager)
        {
            //Debug.Log("death end");
            manager.CameraManager.IsMovable = true;
            manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
        }

        public override void UpdateState(CharacterManager manager)
        {
            //Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
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
