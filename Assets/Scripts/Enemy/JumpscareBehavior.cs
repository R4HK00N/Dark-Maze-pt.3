using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class JumpscareBehavior : MonoBehaviour
{
    public GameObject jumpscareLookAt;
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        StartCoroutine(PitchChange(pitchDelay));

        inJumpscareRange = Physics.CheckSphere(transform.position, jumpscareRange, whatIsPlayer);

        if (inJumpscareRange)
        {
            enemyStompingSFX.Stop();

            Camera.main.transform.LookAt(jumpscareLookAt.transform.position);
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

        SceneManager.LoadScene("MainScene");
    }

    IEnumerator PitchChange(float time)
    {
        yield return new WaitForSeconds(time);
        enemyStompingSFX.pitch = Random.Range(minPitch, maxPitch);
    }
}
