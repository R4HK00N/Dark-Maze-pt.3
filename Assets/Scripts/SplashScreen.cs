using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForSplash());
    }
    public IEnumerator WaitForSplash()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("MainMenu");
    }
}
