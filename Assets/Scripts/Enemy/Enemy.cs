using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float timeToLostPlayer;
    [SerializeField] public float normalSpeed;
    [SerializeField] public float chaseSpeed;
    [SerializeField] public float searchRadius;
    [SerializeField] public int manyPlaceToSearch;
    [SerializeField] public float searchMoveDelay;
    [SerializeField] public float deadDelay;
    [SerializeField] public float angleRotate;
    [SerializeField] public float rotatingSpeed;

    [HideInInspector] public Player player;
    [HideInInspector] public WaypointSet waypointSet;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool isDead;
    [HideInInspector] public DetectingPlayer detectingPlayer;
    [HideInInspector] public EnemySFX enemySFX;
    [HideInInspector] public EnemyWalkSfx enemyWalkSfx;
    public BaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public RetreatState retreatState = new RetreatState();
    public SearchState searchState = new SearchState();
    public DeadState deadState = new DeadState();

    private void Awake()
    {
        isDead = false;
        detectingPlayer = GetComponent<DetectingPlayer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySFX = GetComponentInChildren<EnemySFX>();
        enemyWalkSfx = GetComponentInChildren<EnemyWalkSfx>();
        enemyWalkSfx.Init(navMeshAgent);
        currentState = patrolState;
        patrolState.EnterState(this);
    }

    private void Start()
    {
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
            player.Dead();
        }
    }

    public void DestroyNavMeshAgent()
    {
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            Destroy(collider);
        }
        navMeshAgent.enabled = false;
        animator.applyRootMotion = true;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void WaitForAnimation(Animator animator, string stateName, Action OnCompleted)
    {
        StartCoroutine(AnimationCoroutine(animator, stateName, OnCompleted));
    }

    public IEnumerator AnimationCoroutine(Animator animator, string stateName, Action OnCompleted)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        OnCompleted.Invoke();
    }
}
