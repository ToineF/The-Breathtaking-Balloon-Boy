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
        private CharacterManager _manager;


        private void Start()
        {
            _manager = GameManager.Instance.CharacterManager;
            _currentState = FallingState;
            _currentState.EnterState(_manager);
        }

        private void Update()
        {
            _manager.CameraManager.UpdateCameraPosition();
            _currentState.UpdateState(_manager);

            _manager.MovementManager.ResetVelocity();
            _currentState.FixedUpdateState(_manager);
        }

        private void FixedUpdate()
        {
            _manager.MovementManager.ApplyVelocity(_manager);
        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(_manager);
            _manager.CameraManager.UpdateCameraAngle(_manager);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(_manager);

            _currentState = state;

            _currentState.EnterState(_manager);
        }
    }
}
