using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("General")]
    [SerializeField] private GameObject PauseUIMenu;

    [Header("Sound")]
    [SerializeField] private Slider Slider;
    [SerializeField] private TMP_Text SoundLevelText;

    private void Start()
    {
        if (AudioManager.Instance.MainMusicSource != null)
            Slider.value = AudioManager.Instance.MainMusicSource.volume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        SoundLevelText.text = Mathf.Round(Slider.value * 100).ToString();
        //SPECIFIC SCENES WHERE WE DONT WANT THE PLAYER TO PAUSE
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        PauseUIMenu.SetActive(false);
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        PauseUIMenu.SetActive(true);
    }
}
