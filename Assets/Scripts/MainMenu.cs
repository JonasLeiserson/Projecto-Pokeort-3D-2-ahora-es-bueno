using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string file;

    public void PlayGame(Button button)
    {
        switch (button.tag)
        {
            case "File 1":
                file = GameManager.instance.file1;
                break;
            case "File 2":
                file = GameManager.instance.file2;
                break;
            case "File 3":
                file = GameManager.instance.file3;
                break;
        }

        GameManager.instance.data = SaveSystem.InitSaveFileIfNeeded(file);
        GameManager.instance.AssignData(GameManager.instance.data);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
