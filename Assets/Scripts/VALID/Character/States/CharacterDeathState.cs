using UnityEngine;

namespace BlownAway.Character.States
{
    public class CharacterDeathState : CharacterBaseState
    {
        private float _timer;

        public override void EnterState(CharacterManager manager)
        {
            Debug.Log("DEATH");
            _timer = manager.Transition.FadeTime;
            manager.Transition.SetTransition(() =>
            {
                manager.Transition.PlayFadeIn();


            });

        }

        public override void ExitState(CharacterManager manager)
        {
            Debug.Log("death end");

        }

        public override void UpdateState(CharacterManager manager)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                manager.CheckpointManager.SetToCheckpointPosition(manager.CharacterCollider.Rigidbody.gameObject);
                manager.States.SwitchState(manager.States.FallingState);
            }
        }

        public override void FixedUpdateState(CharacterManager manager)
        {
        }
        public override void LateUpdateState(CharacterManager manager)
        {
        }
    }
}
