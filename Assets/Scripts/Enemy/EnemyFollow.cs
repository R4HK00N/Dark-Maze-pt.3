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

    Shake shake;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {

        StartCoroutine(PitchChange(pitchDelay));
        if (!inJumpscareRange)
            enemy.SetDestination(player.transform.position);

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
