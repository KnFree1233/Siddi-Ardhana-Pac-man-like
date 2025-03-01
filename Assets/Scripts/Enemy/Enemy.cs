using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List<Transform> waypoints = new List<Transform>();
    [Range(0, 360)][SerializeField] public float fovAngle;
    [SerializeField] public float radius;
    [SerializeField] public Player player;
    [SerializeField] public LayerMask obstacleLayer;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public float timeToLostPlayer;
    [SerializeField] public float normalSpeed;
    [SerializeField] public float chaseSpeed;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    private BaseState currentState;
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public RetreatState retreatState = new RetreatState();

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = patrolState;
        patrolState.EnterState(this);
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnPowerUpStart += StartRetreating;
            player.OnPowerUpStop += StopRetreating;
        }
    }

    private void Update()
    {
        //**NOT MY CODE**//
        //Helper to see radius collider and fov raycast enemy in scene view during play mode
        DrawColliderLine.DrawFOVArcWithLines(transform.position, transform.forward, fovAngle, radius, Color.red, 0.1f, 20);
        DrawColliderLine.DrawOverlapSphere(transform.position, radius, Color.white, 0.1f);
        //**//
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
        SwitchState(patrolState);
    }
}
