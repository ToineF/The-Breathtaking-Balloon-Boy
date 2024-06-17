using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class FMODZone : BoxTrigger
{
    [Header("FMOD Params")]
    [SerializeField] private StudioEventEmitter _emmiter;

    private new void Awake()
    {
        base.Awake();
        OnEnterTrigger += OpenTrack;
        OnExitTrigger += CloseTrack;
    }

    private void UpdateTrack(float volume)
    {
        _emmiter.SetParameter("isActive", volume);
    }

    private void OpenTrack() => UpdateTrack(1f);
    private void CloseTrack() => UpdateTrack(0f);

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O)) OpenTrack();
        if (Input.GetKeyUp(KeyCode.P)) CloseTrack();

    }
}
