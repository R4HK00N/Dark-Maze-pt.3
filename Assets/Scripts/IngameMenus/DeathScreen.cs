using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void RespwanButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.SetInt("introbeenplayed", 0);
    }
}
