using UnityEngine;

[System.Serializable]
public class AudioSourceSFX
{
    public string name;
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
