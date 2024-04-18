using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace BlownAway.Character
{
    public class PauseMenu : CharacterSubComponent
    {
        public bool GameIsPaused { get; private set; } = false;


        [SerializeField] private CanvasGroup PauseUIMenu;
        [SerializeField] private GameObject FirstSelectedButton;

        // Inputs
        private PlayerInputs _inputs;

        private void Awake()
        {
            _inputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            _inputs.Enable();
            _inputs.Player.Pause.performed += StartTogglePause;
        }
        private void OnDisable()
        {
            _inputs.Disable();
            _inputs.Player.Pause.performed -= StartTogglePause;
        }

        private void StartTogglePause(InputAction.CallbackContext context)
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
        //SPECIFIC SCENES WHERE WE DONT WANT THE PLAYER TO PAUSE
        // + specific moments when you don't want the player to be able to pause (ex : Quit Game Transition)


        public void Resume()
        {
            Time.timeScale = 1f;
            Manager.Inputs.EnableInputs(true);
            GameIsPaused = false;
            PauseUIMenu.alpha = 0f;
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            Manager.Inputs.EnableInputs(false);
            GameIsPaused = true;
            PauseUIMenu.alpha = 1f;
        }
    }

}