using UnityEngine;
using AntoineFoucault.Utilities;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("General")]
    [SerializeField] private GameObject PauseUIMenu;
    [SerializeField] private GameObject[] PauseUISubMenus;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        PauseUIMenu.SetActive(false);
        PauseUISubMenus.SetAllActive(false);
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        PauseUIMenu.SetActive(true);
    }
}
