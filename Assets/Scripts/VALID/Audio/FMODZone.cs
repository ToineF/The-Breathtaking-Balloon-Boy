using FMODUnity;
using UnityEngine;

public class FMODZone : BoxTrigger
{
    [Header("FMOD Params")]
    [SerializeField] private StudioEventEmitter[] _emmiters;

    private new void Awake()
    {
        base.Awake();
        OnEnterTrigger += OpenTrack;
        OnExitTrigger += CloseTrack;
    }

    private void UpdateTrack(float volume)
    {
        foreach (var emmiter in _emmiters)
        {
            emmiter.SetParameter("isActive", volume);
        }
    }

    private void OpenTrack() => UpdateTrack(1f);
    private void CloseTrack() => UpdateTrack(0f);
}
