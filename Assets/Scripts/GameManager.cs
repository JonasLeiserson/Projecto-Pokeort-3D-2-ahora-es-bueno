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

    public List<PokeortInstance> pokeorts;
    public Pokedex pokedex;

    public Vector3 playerPosition;
    public List<Item> inventory;
    public string saveFile;

    public List<string> trainersDefeated = new List<string>();
    public List<string> gymsDefeated = new List<string>();

    public bool playing;
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
        PersistentRoot.Instance.AddToRoot(this.gameObject);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            RefreshData();
        }

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

        data.pokeorts = pokeorts;
        data.playerPosition = playerPosition;
        data.inventory = inventory;
        data.saveFile = saveFile;
        data.trainersDefeated = trainersDefeated;
        data.gymsDefeated = gymsDefeated;
    }

    public void AssignData(SaveData saveData)
    {
        pokeorts = saveData.pokeorts;
        playerPosition = saveData.playerPosition;
        inventory = saveData.inventory;
        saveFile = saveData.saveFile;
        trainersDefeated = saveData.trainersDefeated;
        gymsDefeated = saveData.gymsDefeated;

        pokedex.pokeorts = pokeorts;
    }

    public void SaveGame()
    {
        RefreshData();
        SaveSystem.SaveGame(data);
        Debug.Log("Juego guardado.");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playing)
        {
            if (scene.name == "GameScene")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = playerPosition;
                }

                PokedexPlayerManager.instance.pokedex = pokedex;
                Inventario.instance.items = inventory;

                foreach (string trainer in trainersDefeated)
                {
                    GameObject trainerGO = GameObject.FindGameObjectWithTag(trainer);
                    trainerGO.GetComponent<CombatNPCInteraction>().enabled = false;
                }
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
