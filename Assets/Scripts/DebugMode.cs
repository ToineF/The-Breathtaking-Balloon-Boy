using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlownAway.DebugMode {

public class DebugMode : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (SceneManager.GetActiveScene().buildIndex > 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
