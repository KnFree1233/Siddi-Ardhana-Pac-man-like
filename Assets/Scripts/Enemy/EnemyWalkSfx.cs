using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkSfx : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSFX walkSfx;

    private NavMeshAgent navMeshAgent;
    private bool playingSfx;
    private bool isPlaySfx;

    public void Init(NavMeshAgent navMeshAgent)
    {
        this.navMeshAgent = navMeshAgent;
    }

    private void Awake()
    {
        playingSfx = isPlaySfx = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = walkSfx.audioClip;
        audioSource.loop = true;
    }

    private void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f && !navMeshAgent.isStopped)
        {
            playingSfx = true;
        }
        else
        {
            playingSfx = false;
        }

        if (isPlaySfx != playingSfx)
        {
            isPlaySfx = playingSfx;
            ToggleSfx();
        }
    }

    private void ToggleSfx()
    {
        if (isPlaySfx)
        {
            audioSource.Play();
        }
        else if (!isPlaySfx)
        {
            audioSource.Stop();
        }
    }
}
