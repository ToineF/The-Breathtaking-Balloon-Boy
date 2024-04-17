using BlownAway.Character;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlownAway.DebugMode
{

    public class DebugMode : CharacterSubComponent
    {
        private void Awake()
        {
            if (transform.parent == null) DontDestroyOnLoad(this);
        }

#if UNITY_EDITOR

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
                FindObjectOfType<CharacterUI>()?.ToggleUI();
            }
            if (Input.GetKeyDown(KeyCode.F5)) // Child Level 0
            {
                Manager.ChildrenManager.SetChildren(0);
            }
            if (Input.GetKeyDown(KeyCode.F6)) // Child Level 1
            {
                Manager.ChildrenManager.SetChildren(1);
            }
            if (Input.GetKeyDown(KeyCode.F7)) // Child Level 2
            {
                Manager.ChildrenManager.SetChildren(2);
            }
            if (Input.GetKeyDown(KeyCode.F8)) // Screenshot
            {
                TakeScreenshot();
            }
            if (Input.GetKeyDown(KeyCode.F9)) // God Mode
            {
                Manager.ChildrenManager.PassInGodMode();
            }
        }

        private void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png");
            UnityEditor.AssetDatabase.Refresh();
        }

#endif

    }
}
