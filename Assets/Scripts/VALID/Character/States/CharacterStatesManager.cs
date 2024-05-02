using UnityEngine;

namespace BlownAway.Character.States
{

    public class CharacterStatesManager : CharacterSubComponent
    {
        public Vector3 Position { get; set; }
        public bool StickToGround { get; set; }

        // States Bank
        public CharacterIdleState IdleState = new CharacterIdleState();
        public CharacterWalkingState WalkingState = new CharacterWalkingState();
        public CharacterFallingState FallingState = new CharacterFallingState();
        public CharacterFloatingState FloatingState = new CharacterFloatingState();
        public CharacterPropulsionState PropulsionState = new CharacterPropulsionState();
        public CharacterJumpState JumpState = new CharacterJumpState();
        public CharacterDashState DashState = new CharacterDashState();
        public CharacterGroundPoundState GroundPoundState = new CharacterGroundPoundState();
        public CharacterDeathState DeathState = new CharacterDeathState();
        public CharacterCutsceneState CutsceneState = new CharacterCutsceneState();


        private CharacterBaseState _currentState;


        protected override void StartScript(CharacterManager manager)
        {
            _currentState = FallingState;
            _currentState.EnterState(Manager, null);
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
            if (StickToGround)
            {
                Manager.MovementManager.TeleportPlayer(Manager, Position);
                StickToGround = false;
            }


        }

        private void LateUpdate()
        {
            _currentState.LateUpdateState(Manager);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(Manager);

            CharacterBaseState previousState = _currentState;

            _currentState = state;

            _currentState.EnterState(Manager, previousState);
        }

        public bool IsInState(CharacterBaseState state)
        {
            return _currentState == state;
        }
    }
}
