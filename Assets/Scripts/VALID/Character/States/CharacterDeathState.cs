using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("DEATH");
            manager.Transition.SetTransition(() =>
            {
                manager.Transition.PlayFadeIn();
                //manager.States.SwitchState(manager.States.FallingState);
                Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);

                manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
                Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
                Debug.Log("AAA " + manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);
            });

            Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);

        }

        public override void ExitState(CharacterManager manager)
        {
            Debug.Log("death end");

        }

        public override void UpdateState(CharacterManager manager)
        {
            Debug.Log(manager.CharacterCollider.Rigidbody.gameObject.name + ": " + manager.CharacterCollider.Rigidbody.gameObject.transform.position);

        }

        public override void FixedUpdateState(CharacterManager manager)
        {
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
