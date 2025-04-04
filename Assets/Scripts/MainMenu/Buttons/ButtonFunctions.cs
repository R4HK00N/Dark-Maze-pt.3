using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject settingsScreen;
    public GameObject creditsScreen;

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

    public void SettingsButton()
    {
        mainButtons.SetActive(false);
        settingsScreen.SetActive(true);
    }
    public void CreditsButton()
    {
        mainButtons.SetActive(false);
        creditsScreen.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void GoBackFromSettings()
    {
        settingsScreen.SetActive(false);
        mainButtons.SetActive(true);
    }
    public void GoBackFromCredits()
    {
        creditsScreen.SetActive(false);
        mainButtons.SetActive(true);
    }
}
