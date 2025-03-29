using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Range(0, 360)][SerializeField] public float fovAngle;
    [SerializeField] public float radius;
    [SerializeField] public LayerMask obstacleLayer;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public float timeToLostPlayer;
    [SerializeField] public float normalSpeed;
    [SerializeField] public float chaseSpeed;
    [SerializeField] public float searchRadius;
    [SerializeField] public int manyPlaceToSearch;
    [SerializeField] public float searchMoveDelay;
    [SerializeField] public float deadDelay;
    [SerializeField] public float angleRotate;
    [SerializeField] public float rotatingSpeed;
    [SerializeField] public float noiseTolerance;

    [HideInInspector] public Player player;
    [HideInInspector] public List<Transform> waypoints = new List<Transform>();
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool isDead;
    [HideInInspector] public DetectingPlayer detectingPlayer;
    [HideInInspector] public AudioSource foundPlayerSFX;
    private BaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public RetreatState retreatState = new RetreatState();
    public SearchState searchState = new SearchState();
    public DeadState deadState = new DeadState();

    private void Awake()
    {
        isDead = false;
        target = null;
        detectingPlayer = GetComponent<DetectingPlayer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = patrolState;
        patrolState.EnterState(this);
    }

    private void Start()
    {
        foundPlayerSFX = GameObject.Find("Found Player SFX").GetComponent<AudioSource>();
        navMeshAgent.avoidancePriority = Random.Range(30, 80);
        if (player != null)
        {
            player.OnPowerUpStart += StartRetreating;
            player.OnPowerUpStop += StopRetreating;
        }
    }

    private void Update()
    {
        animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(BaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    private void StartRetreating()
    {
        SwitchState(retreatState);
    }

    private void StopRetreating()
    {
        if (!isDead) SwitchState(patrolState);
    }

    public void Dead()
    {
        SwitchState(deadState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.isInvisible || player.isPowerUp) return;

        if (other.gameObject.CompareTag("Player"))
        {
            player.Dead(1);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
