using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [SerializeField] List<AudioSourceSFX> audioSourceList;

    public static SfxManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayAudio(string name)
    {
        AudioSourceSFX audio = audioSourceList.Find(audio => audio.name == name);
        audio?.Play();
    }

    public void StopAudio()
    {
        AudioSourceSFX audio = audioSourceList.Find(audio => audio.name == name);
        audio?.Stop();
    }
}
