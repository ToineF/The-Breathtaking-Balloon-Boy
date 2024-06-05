using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace BlownAway.Character
{
    public class PauseMenu : SubMenu
    {
        public UnityEvent OnPause;
        public UnityEvent OnResume;
        public bool GameIsPaused { get; private set; } = false;

        [Header("References")]
        [SerializeField] private CanvasGroup _globalPauseUIMenu;

        [Header("Options Menu")]
        [SerializeField] private SubMenu[] _subMenusToClose;

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

        protected override void StartScript(CharacterManager manager)
        {
            base.StartScript(manager);
            _globalPauseUIMenu.alpha = 0;
        }

        private void StartTogglePause(InputAction.CallbackContext context)
        {
            if (Manager.States.IsInMovableState() == false) return; //SPECIFIC SCENES WHERE WE DONT WANT THE PLAYER TO PAUSE
                                                                    // + specific moments when you don't want the player to be able to pause (ex : Quit Game Transition)

            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
        


        public void Resume()
        {
            Time.timeScale = 1f;
            Manager.Inputs.EnableInputs(true);
            GameIsPaused = false;
            CloseMenu(_globalPauseUIMenu);
            foreach (var submenu in _subMenusToClose)
            {
                CloseMenu(submenu.CanvasGroup);
            }
            Manager.CameraManager.SetCursorVisible(false);
            OnResume?.Invoke();
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            Manager.Inputs.EnableInputs(false);
            GameIsPaused = true;
            OpenMenu(_globalPauseUIMenu);
            OpenMenu(CanvasGroup, FirstSelectedButton);
            Manager.CameraManager.SetCursorVisible(true);
            OnPause?.Invoke();
        }
    }
}