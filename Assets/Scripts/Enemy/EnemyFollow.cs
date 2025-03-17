using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public GameObject player;
    public LayerMask whatIsPlayer;
    public float jumpscareRange;
    public float jumpscareDuration;
    bool inJumpscareRange;

    public AudioSource enemyStompingSFX;
    public float pitchDelay;
    public float minPitch;
    public float maxPitch;

    public float followDistance = 10f;
    public float wanderRange = 20f;
    public float wanderInterval = 3f;

    private float nextWanderTime;

    Shake shake;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        StartCoroutine(PitchChange(pitchDelay));
        if (!inJumpscareRange)
        {
            if (distanceToPlayer <= followDistance)
            {
                // Follow the player if within range
                enemy.SetDestination(player.transform.position);
            }
            else
            {
                // If player is not near, wander randomly
                if (Time.time >= nextWanderTime)
                {
                    Wander();
                    nextWanderTime = Time.time + wanderInterval;
                }
            }
        }

        inJumpscareRange = Physics.CheckSphere(transform.position, jumpscareRange, whatIsPlayer);

        if (inJumpscareRange)
        {
            enemyStompingSFX.Stop();

            Camera.main.transform.LookAt(transform.position);
            enemy.enabled = false;
            player.GetComponent<Movement>().enabled = false;

            shake = player.GetComponentInChildren<Shake>();
            shake.canShake = false;

            //Ienumerator voor alles wat na jumpscare gebeurt
            StartCoroutine(JumpscareEnd(jumpscareDuration));

            //alles voor jumpscare trigger
        }
    }
    void Wander()
    {
        // Generate a random position within a defined range from the enemy's current position
        Vector3 randomPosition = RandomNavMeshLocation(wanderRange);

        // Set the NavMeshAgent destination to this random position
        enemy.SetDestination(randomPosition);
    }

    Vector3 RandomNavMeshLocation(float range)
    {
        // Generate a random point within a specified range on the NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;
        NavMeshHit hit;

        // Find the closest point on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;  // Fallback to the current position if no valid NavMesh point is found
    }

    IEnumerator JumpscareEnd(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("robbert scene");
        
    }

    IEnumerator PitchChange(float time)
    {
        yield return new WaitForSeconds(time);
        enemyStompingSFX.pitch = Random.Range(minPitch, maxPitch);
    }
}
