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
        Pokedex selectedPokedex = null;

        switch (button.tag)
        {
            case "File 1":
                file = GameManager.instance.file1;
                selectedPokedex = GameManager.instance.PokedexPlayer1;
                break;
            case "File 2":
                file = GameManager.instance.file2;
                selectedPokedex = GameManager.instance.PokedexPlayer2;
                break;
            case "File 3":
                file = GameManager.instance.file3;
                selectedPokedex = GameManager.instance.PokedexPlayer3;
                break;
        }

        if (selectedPokedex == null)
        {
            Debug.LogError("Invalid button tag or uninitialized Pokedex.");
            return;
        }

        GameManager.instance.data = SaveSystem.InitSaveFileIfNeeded(file);
        GameManager.instance.data.pokedex = selectedPokedex;
        GameManager.instance.AssignData(GameManager.instance.data);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
