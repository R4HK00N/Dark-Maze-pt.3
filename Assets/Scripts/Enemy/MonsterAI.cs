using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public float updateInterval = 0.5f;   // How often to update path
    public float turnSpeed = 5f;          // Consistent turning speed
    public float detectionRange = 10f;    // Distance to detect player
    public float wanderRadius = 20f;      // Max distance for wandering
    public float cornerSlowdownDistance = 3f; // Distance to slow down near corners
    public float minSpeed = 3f;         // Minimum speed near corners

    private Transform player;             // Player transform, found at runtime
    private NavMeshAgent agent;
    private float updateTimer;
    private Vector3 lastPlayerPosition;
    private bool isChasing = false;
    public float baseSpeed = 7f;       // Normal speed

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing from " + gameObject.name);
            enabled = false;
            return;
        }

        // Find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
            enabled = false;
            return;
        }

        agent.updateRotation = false;
        agent.speed = baseSpeed;
        agent.angularSpeed = 360f;
        updateTimer = Random.Range(0f, updateInterval);

        // Start wandering immediately
        SetWanderDestination();
    }

    void Update()
    {
        if (player == null) return;

        // Check if player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerIsNear = distanceToPlayer <= detectionRange;

        if (playerIsNear && !isChasing)
        {
            isChasing = true;
            agent.isStopped = false;
        }
        else if (!playerIsNear && isChasing)
        {
            isChasing = false;
            SetWanderDestination();
        }

        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            UpdatePath();
            updateTimer = updateInterval;
        }

        AdjustSpeedNearCorners();
        SmoothTurn();
    }

    void UpdatePath()
    {
        if (isChasing)
        {
            if (Vector3.Distance(lastPlayerPosition, player.position) > 1f)
            {
                agent.SetDestination(player.position);
                lastPlayerPosition = player.position;
            }
        }
        else
        {
            if (!agent.hasPath || agent.remainingDistance < 1f)
            {
                SetWanderDestination();
            }
        }
    }

    void SetWanderDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        randomPoint.y = transform.position.y; // Keep it on the same plane

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void AdjustSpeedNearCorners()
    {
        if (agent.hasPath && agent.path.corners.Length > 1)
        {
            float distanceToNextCorner = Vector3.Distance(transform.position, agent.path.corners[1]);
            if (distanceToNextCorner < cornerSlowdownDistance)
            {
                // Smoothly reduce speed near corners
                float speedFactor = distanceToNextCorner / cornerSlowdownDistance;
                agent.speed = Mathf.Lerp(minSpeed, baseSpeed, speedFactor);
            }
            else
            {
                agent.speed = baseSpeed;
            }
        }
    }

    void SmoothTurn()
    {
        if (agent.velocity.sqrMagnitude > 0.1f && agent.hasPath)
        {
            Vector3 velocityDir = agent.velocity.normalized;
            Vector3 nextCornerDir = (agent.path.corners[1] - transform.position).normalized;

            // Increase responsiveness at sharp turns
            float angleToCorner = Vector3.Angle(velocityDir, nextCornerDir);
            float turnFactor = Mathf.Clamp01(angleToCorner / 90f); // More turn influence at sharp angles
            Vector3 targetDir = Vector3.Lerp(velocityDir, nextCornerDir, 0.5f + turnFactor * 0.5f).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    void OnDrawGizmos()
    {
        if (agent != null && agent.hasPath)
        {
            Gizmos.color = isChasing ? Color.red : Color.green;
            Vector3[] corners = agent.path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
            }
        }
    }
}