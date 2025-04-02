using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public Animator canvasAnimator;
    public float waitFadeinTime;
    public void StartButton()
    {
        StartCoroutine(WaitForFadeIn(waitFadeinTime));

        canvasAnimator.SetInteger("StartFade", 1);
    }
    IEnumerator WaitForFadeIn(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene("MainScene");
    }
}
