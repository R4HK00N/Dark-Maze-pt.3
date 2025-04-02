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
    public bool canJumpScare = true;

    GameObject deathScreen;
    GameObject phone;
    GameObject ingameUI;
    public Animator jumpscareAnimator;
    public SkinnedMeshRenderer[] monsterParts;
    public GameObject jumpscareModel;
    public AudioSource enemyStompingSFX;
    public float pitchDelay;
    public float minPitch;
    public float maxPitch;

    Shake shake;
    GameObject camera;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        jumpscareModel = player.transform.Find("Full_WENDIGO").gameObject;
        jumpscareAnimator = player.GetComponent<Animator>();
        ingameUI = GameObject.FindGameObjectWithTag("IngameUI");
        phone = ingameUI.transform.Find("PhoneMap").gameObject;
        deathScreen = ingameUI.transform.Find("DeathScreen").gameObject;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        StartCoroutine(PitchChange(pitchDelay));

        if(canJumpScare)
            inJumpscareRange = Physics.CheckSphere(transform.position, jumpscareRange, whatIsPlayer);

        if (inJumpscareRange)
        {
            canJumpScare = false;
            jumpscareModel.SetActive(true);
            phone.SetActive(false);

            foreach(SkinnedMeshRenderer renderer in monsterParts)
            {
                renderer.enabled = false;
            }

            enemyStompingSFX.Stop();

            //Camera.main.transform.LookAt(jumpscareLookAt.transform.position);
            player.GetComponent<Movement>().enabled = false;

            shake = player.GetComponentInChildren<Shake>();
            shake.canShake = false;

            camera.SetActive(false);

            jumpscareAnimator.SetInteger("Jumpscare", 1);

            //Ienumerator voor alles wat na jumpscare gebeurt
            StartCoroutine(JumpscareEnd(jumpscareDuration));

            //alles voor jumpscare trigger
        }
    }
    IEnumerator JumpscareEnd(float time)
    {
        yield return new WaitForSeconds(time);

        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        //SceneManager.LoadScene("MainScene");
    }

    IEnumerator PitchChange(float time)
    {
        yield return new WaitForSeconds(time);
        enemyStompingSFX.pitch = Random.Range(minPitch, maxPitch);
    }
}
