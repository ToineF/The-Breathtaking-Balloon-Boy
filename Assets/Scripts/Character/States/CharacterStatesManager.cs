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
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        private void FixedUpdate()
        {
            CharacterManager.Instance.ResetVelocity();
            _currentState.FixedUpdateState(this);
            CharacterManager.Instance.ApplyVelocity();
        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(this);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(this);

            _currentState = state;

            _currentState.EnterState(this);
        }
    }
}
