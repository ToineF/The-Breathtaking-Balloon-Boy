using UnityEngine;

namespace BlownAway.Character.States
{

    public class CharacterStatesManager : MonoBehaviour
    {

        // States Bank
        public CharacterIdleState IdleState = new CharacterIdleState();
        public CharacterWalkingState WalkingState = new CharacterWalkingState();
        public CharacterFallingState FallingState = new CharacterFallingState();
        public CharacterFloatingState FloatingState = new CharacterFloatingState();
        public CharacterPropulsionState PropulsionState = new CharacterPropulsionState();
        public CharacterDeathState DeathState = new CharacterDeathState();
        public CharacterCutsceneState CutsceneState = new CharacterCutsceneState();


        private CharacterBaseState _currentState;


        private void Start()
        {
            _currentState = FallingState;
            _currentState.EnterState(CharacterManager.Instance);
        }

        private void Update()
        {
            _currentState.UpdateState(CharacterManager.Instance);
        }

        private void FixedUpdate()
        {
            CharacterManager.Instance.MovementManager.ResetVelocity();
            _currentState.FixedUpdateState(CharacterManager.Instance);
            CharacterManager.Instance.MovementManager.ApplyVelocity();
        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(CharacterManager.Instance);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(CharacterManager.Instance);

            _currentState = state;

            _currentState.EnterState(CharacterManager.Instance);
        }
    }
}
