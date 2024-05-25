using UnityEngine;
using FMOD.Studio;
using UnityEngine.UI;

public class FMODBus : MonoBehaviour
{
    private Bus _bus;
    [SerializeField] private string _name;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(UpdateBus);
    }

    private void Start()
    {
        _bus = FMODUnity.RuntimeManager.GetBus($"bus:/{_name}");
    }

    private void UpdateBus(float busVolume)
    {
        _bus.setVolume(DecibelToLinear(busVolume));
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20f);
        return linear;
    }
}
