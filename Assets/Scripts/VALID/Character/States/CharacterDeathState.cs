using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        public override bool IsMovable { get => false; }

        public override void EnterState(CharacterManager manager, CharacterBaseState previousState)
        {
            Debug.Log("DEATH");
            manager.MovementManager.ResetAllExternalForces();
            manager.CameraManager.IsMovable = false;
            manager.MovementManager.OnDeath?.Invoke(manager);
            //manager.Transition.PlayFakeTransition();
            //manager.MovementManager.StartDeath(manager);

            manager.Transition.SetTransition(() =>
            {
                manager.Transition.PlayFadeIn();
                Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
                manager.CameraManager.IsMovable = true;

                manager.CharacterCollider.Rigidbody.gameObject.SetActive(false);
                manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
                manager.CharacterCollider.Rigidbody.gameObject.SetActive(true);
                Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
                manager.States.SwitchState(manager.States.FallingState);
                //manager.Transition.PlayFadeIn();


                Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            });

            //manager.States.SwitchState(manager.States.FallingState);
            //{

            //});

            //Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            //manager.CameraManager.IsMovable = true;
            //manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.transform);
            //manager.States.SwitchState(manager.States.FallingState);

            // Feedbacks
            manager.Feedbacks.PlayFeedback(manager.Data.FeedbacksData.DeathFeedback);

        }

        public override void ExitState(CharacterManager manager)
        {
            //Debug.Log("death end");

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
