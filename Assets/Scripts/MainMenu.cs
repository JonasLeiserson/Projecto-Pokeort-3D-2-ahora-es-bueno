using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CargarFileButtons(GameObject fileButtons, GameObject menuButtons) 
    {
        fileButtons.SetActive(true);
        menuButtons.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
