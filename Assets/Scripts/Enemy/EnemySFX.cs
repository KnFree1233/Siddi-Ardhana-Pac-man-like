using System.Collections;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioSFX[] idleSounds;

    int currSfxIndex;
    private bool isPlaySfx;
    private bool playingSfx;
    private Coroutine currCoroutine;

    private void Awake()
    {
        isPlaySfx = true;
        playingSfx = false;
    }

    private void Update()
    {
        if(isPlaySfx) PlayingSfx();
    }

    private void PlayingSfx() 
    {
        if(!playingSfx) currCoroutine = StartCoroutine(WaitSFXFinish(audioSource));
    }

    IEnumerator WaitSFXFinish(AudioSource audioSource) 
    {
        playingSfx = true;
        currSfxIndex = Random.Range(0, idleSounds.Length);
        audioSource.clip = idleSounds[currSfxIndex].audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.Stop();
        playingSfx = false;
        currSfxIndex++;
    }

    public void PlaySfx() 
    {
        isPlaySfx = true;
    }

    public void StopSfx() 
    {
        isPlaySfx = false;
        if(currCoroutine != null) StopCoroutine(currCoroutine);
        currCoroutine = null;
        audioSource.Stop();
    }
}
