using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public float chaseRange = 10f; // Distance at which the enemy enters chase mode
    public float patrolRange = 5f; // Distance at which the enemy starts patrolling again
    public Transform[] patrolPoints; // Array of patrol points

    private int currentPatrolPoint = 0; // Index of the current patrol point
    private bool isChasing = false; // Flag to track chase mode

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned to EnemyAI!");
        }
    }

    void Update()
    {
        
        if (!player)
        {
            Debug.LogError("No player object assigned to EnemyAI!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isChasing)
        {
            // Chase the player
            if (distanceToPlayer > patrolRange)
            {
                isChasing = false; // Stop chasing if player gets too far
            }
            else
            {
                agent.SetDestination(player.position); // Move towards the player
            }
        }
        else
        {
            // Patrol
            if (distanceToPlayer <= chaseRange)
            {
                isChasing = true; // Start chasing if player gets close
            }
            else
            {
                
                Patrol(); // Move to the next patrol point
            }
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
        {
            return; // No patrol points, so don't patrol
        }

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) <= agent.stoppingDistance)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length; // Move to the next patrol point
        }

        agent.SetDestination(patrolPoints[currentPatrolPoint].position); // Move towards the current patrol point
    }
}
