using UnityEngine;
using FMODUnity;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayClip(EventReference eventPath, Vector3 position = default)
    {
        if (eventPath.IsNull) return;
        RuntimeManager.PlayOneShot(eventPath, position);
    }
}
