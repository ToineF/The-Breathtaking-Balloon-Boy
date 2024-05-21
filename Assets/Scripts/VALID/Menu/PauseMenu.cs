using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace BlownAway.Character
{
    public class PauseMenu : CharacterSubComponent
    {
        public bool GameIsPaused { get; private set; } = false;

        [Header("References")]
        [SerializeField] private CanvasGroup GlobalPauseUIMenu;

        [Header("Main Menu")]
        [SerializeField] private CanvasGroup MainPauseUIMenu;
        [SerializeField] private GameObject FirstSelectedButton;

        [Header("Options Menu")]
        [SerializeField] private CanvasGroup OptionsPauseUIMenu;
        [SerializeField] private GameObject FirstOptionsSelectedButton, MainMenuOptionsSelectedButton;

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
            GlobalPauseUIMenu.alpha = 0;
        }

        private void StartTogglePause(InputAction.CallbackContext context)
        {
            if (Manager.States.IsInState(Manager.States.CutsceneState)) return; //SPECIFIC SCENES WHERE WE DONT WANT THE PLAYER TO PAUSE
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
            CloseMenu(GlobalPauseUIMenu);
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            Manager.Inputs.EnableInputs(false);
            GameIsPaused = true;
            OpenMenu(GlobalPauseUIMenu);
            OpenMenu(MainPauseUIMenu, FirstSelectedButton);
        }

        public void OpenOptions()
        {
            OpenMenu(OptionsPauseUIMenu, FirstOptionsSelectedButton);
            CloseMenu(MainPauseUIMenu);
        }

        public void CloseOptions()
        {
            OpenMenu(MainPauseUIMenu, MainMenuOptionsSelectedButton);
            CloseMenu(OptionsPauseUIMenu);
        }

        private void OpenMenu(CanvasGroup group, GameObject firstSelected = null)
        {
            group.interactable = true;
            group.alpha = 1;
            group.blocksRaycasts = true;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        private void CloseMenu(CanvasGroup group, GameObject firstSelected = null)
        {
            group.interactable = false;
            group.alpha = 0;
            group.blocksRaycasts = false;
            if (firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

}