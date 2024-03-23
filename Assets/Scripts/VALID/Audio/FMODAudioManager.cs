using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager Instance;

    public AudioSource MainMusicSource { get => _mainMusicSource; private set => _mainMusicSource = value; }
    public AudioSource SfxSource { get => _sfxSource; private set => _sfxSource = value; }

    [SerializeField] protected AudioSource _mainMusicSource;
    [SerializeField] protected AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlayClip(string eventPath, Vector3 position = default)
    {
        RuntimeManager.PlayOneShot(eventPath, position);
    }
}
