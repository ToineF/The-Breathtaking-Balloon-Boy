using UnityEngine;

namespace BlownAway.Character
{
    public class PauseMenu : CharacterSubComponent
    {
        public bool GameIsPaused { get; private set; } = false;

        [Header("General")]
        [SerializeField] private CanvasGroup PauseUIMenu;

        private void Update()
        {
            if (Manager.Inputs.TogglePause)
            {
                if (GameIsPaused)
                    Resume();
                else
                    Pause();
            }
            //SPECIFIC SCENES WHERE WE DONT WANT THE PLAYER TO PAUSE
            // + specific moments when you don't want the player to be able to pause (ex : Quit Game Transition)
        }

        public void Resume()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
            PauseUIMenu.alpha = 0f;
        }

        public void Pause()
        {
            GameIsPaused = true;
            Time.timeScale = 0f;
            PauseUIMenu.alpha = 1f;
        }
    }
}