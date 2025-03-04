using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicHandler : MonoBehaviour
{
    public GameManager gameManager;
    public Animator introAnimator;
    public GameObject[] introCinematicobjects;
    public GameObject[] whilePlayingObjects;

    float currentWaitingTime;
    public float introCinematicDuration;
    public float outroCinematicDuration;
    public float jumpscareCinematicDuration;


    bool introIsplaying;
    bool outroIsplaying;
    public bool jumpscareIsPlaying;

    public void Start()
    {
        introIsplaying = true;
        currentWaitingTime = introCinematicDuration;
        StartCoroutine(WaitForCinematic(currentWaitingTime));
    }
    IEnumerator WaitForCinematic(float time)
    {
        yield return new WaitForSeconds(time);
        if (introIsplaying)
        {
            introIsplaying = false;

            foreach(GameObject introObject in introCinematicobjects)
            {
                introObject.SetActive(false);
            }
            foreach (GameObject playingObject in whilePlayingObjects)
            {
                playingObject.SetActive(true);
            }

            gameManager.SpawnEnemies();
        }
    }
}
