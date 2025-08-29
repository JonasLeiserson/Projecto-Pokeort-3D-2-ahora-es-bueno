using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string file1;
    public string file2;
    public string file3;

    public SaveData data;

    public Pokedex pokedex;
    public Pokedex PokedexPlayer1;
    public Pokedex PokedexPlayer2;
    public Pokedex PokedexPlayer3;

    public Vector3 playerPosition;
    public List<Item> inventory;
    public string saveFile;
    
    void Awake() 
    {
        file1 = "1.json";
        file2 = "2.json";
        file3 = "3.json";

        if (instance != null && instance != this)
	    {
	        Destroy(this.gameObject);
	    }
	    else
	    {
	        instance = this;
	    }

	    DontDestroyOnLoad(this.gameObject);
        RefreshData();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void RefreshData()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        if (Inventario.instance != null)
        {
            inventory = Inventario.instance.items;
        }

        data.pokedex = pokedex;
        data.playerPosition = playerPosition;
        data.inventory = inventory;
        data.saveFile = saveFile;
    }

    public void AssignData(SaveData saveData) 
    {
	    pokedex = saveData.pokedex;
	    playerPosition = saveData.playerPosition;
	    inventory = saveData.inventory;
	    saveFile = saveData.saveFile;
    }    

    public void SaveGame()
    {
        RefreshData();
        SaveSystem.SaveGame(data);
        Debug.Log("Juego guardado.");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }

            Inventario.instance.items = inventory;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
