using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlownAway.DebugMode
{

    public class DebugMode : MonoBehaviour
    {
        private bool _canvasHidden = false;
        private List<Canvas> _canvasList = new List<Canvas>();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

#if UNITY_EDITOR

        private void Start()
        {
            UpdateCanvasList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1)) // Previous Scene
            {
                if (SceneManager.GetActiveScene().buildIndex > 0)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
            if (Input.GetKeyDown(KeyCode.F2)) // Restart Scene
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.F3)) // Next Scene
            {
                if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (Input.GetKeyDown (KeyCode.F4)) // Remove all UI
            {
                UpdateCanvasList();
                _canvasHidden = !_canvasHidden;
                foreach (Canvas c in _canvasList)
                {
                    c.gameObject.SetActive(_canvasHidden);
                }
            }
        }


        private void UpdateCanvasList()
        {
            Canvas[] canvasList = FindObjectsOfType<Canvas>();
            foreach (Canvas C in canvasList)
            {
                if (_canvasList.Contains(C)) continue;
                _canvasList.Add(C);
            }
        }
#endif

    }
}
