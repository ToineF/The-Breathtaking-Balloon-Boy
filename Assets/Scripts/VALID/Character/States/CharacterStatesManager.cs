using UnityEngine;

namespace BlownAway.Character.States
{

    public class CharacterStatesManager : MonoBehaviour
    {
        public CharacterManager Manager { get; set; }

        // States Bank
        public CharacterIdleState IdleState = new CharacterIdleState();
        public CharacterWalkingState WalkingState = new CharacterWalkingState();
        public CharacterFallingState FallingState = new CharacterFallingState();
        public CharacterFloatingState FloatingState = new CharacterFloatingState();
        public CharacterPropulsionState PropulsionState = new CharacterPropulsionState();
        public CharacterDeriveState DeriveState = new CharacterDeriveState();
        public CharacterDeathState DeathState = new CharacterDeathState();
        public CharacterCutsceneState CutsceneState = new CharacterCutsceneState();


        private CharacterBaseState _currentState;


        private void Start()
        {
            _currentState = FallingState;
            _currentState.EnterState(Manager);
        }

        private void Update()
        {
            _currentState.UpdateState(Manager);
        }

        private void FixedUpdate()
        {
            Manager.MovementManager.ResetVelocity();

            _currentState.FixedUpdateState(Manager);

            Manager.MovementManager.ApplyVelocity(Manager);
        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(Manager);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(Manager);

            _currentState = state;

            _currentState.EnterState(Manager);
        }
    }
}
