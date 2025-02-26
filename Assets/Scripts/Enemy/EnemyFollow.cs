using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public LayerMask whatIsPlayer;
    public float jumpscareRange;
    public float jumpscareDuration;
    bool inJumpscareRange;

    void Start()
    {
        
    }

    void Update()
    {
        if(!inJumpscareRange)
            enemy.SetDestination(player.position);

        inJumpscareRange = Physics.CheckSphere(transform.position, jumpscareRange, whatIsPlayer);

        if (inJumpscareRange)
        {
            Camera.main.transform.LookAt(transform.position);
            enemy.enabled = false;
            player.GetComponent<Movement>().enabled = false;

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
}
