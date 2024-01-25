using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioSlider : MonoBehaviour
{

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
        SoundLevelText.text = Mathf.Round(Slider.value * 100).ToString();
        if (AudioManager.Instance.MainMusicSource != null)
            AudioManager.Instance.MainMusicSource.volume = Slider.value;
    }
}
