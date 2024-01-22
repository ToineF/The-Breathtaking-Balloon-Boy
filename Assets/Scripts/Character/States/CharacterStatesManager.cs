using UnityEngine;

namespace Character.States
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

        // Inputs
        public PlayerInputs InputActions { get; private set; }


        private CharacterBaseState _currentState;

        #region Inputs
        private void Awake()
        {
            InputActions = new PlayerInputs();
        }
        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }
        #endregion

        private void Start()
        {
            _currentState = IdleState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState(this);
        }

        public void SwitchState(CharacterBaseState state)
        {
            _currentState.ExitState(this);

            _currentState = state;

            _currentState.EnterState(this);
        }
    }
}
