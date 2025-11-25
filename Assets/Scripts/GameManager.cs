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
            if (scene.name == "Combate")
            {
                SaveGame();
                AssignData(data);
            }
            // ⭐ NUEVO: Ejecutar siempre que se cargue GameScene
        }

        if (scene.name == "GameScene")
        {
            // Usar Coroutine para asegurar que los objetos estén cargados
            StartCoroutine(ConfigurarGameScene());
        }
    }

    // ⭐ NUEVO: Método separado para configurar GameScene
    private IEnumerator ConfigurarGameScene()
    {
        // Esperar un frame para asegurar que todos los objetos estén instanciados
        yield return null;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (playerPosition == new Vector3(0, 0, 0))
            {
                playerPosition = new Vector3(223, 6, 83);
            }
            player.transform.position = playerPosition;
            Debug.Log($"Jugador posicionado en: {playerPosition}");
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador en GameScene");
        }

        foreach (string trainer in trainersDefeated)
        {
            Debug.Log($"Procesando entrenador: {trainer}");
            GameObject trainerGO = GameObject.FindGameObjectWithTag(trainer);
            
            if (trainerGO != null)
            {
                CombatNPCInteraction combatComponent = trainerGO.GetComponentInChildren<CombatNPCInteraction>();
                if (combatComponent != null)
                {
                    trainerGO.GetComponentInChildren<CombatNPCInteraction>().gameObject.SetActive(false);
                    Debug.Log("Begetativo");
                }

                VisionNPC visionComponent = trainerGO.GetComponentInChildren<VisionNPC>();
                if (visionComponent != null)
                {
                    visionComponent.gameObject.SetActive(false);
                }

                // Activar diálogo si existe
                DialogoTrigger dialogoComponent = trainerGO.GetComponent<DialogoTrigger>();
                if (dialogoComponent != null)
                {
                    dialogoComponent.enabled = true;
                }
                
                Debug.Log($"Entrenador {trainer} configurado correctamente");
            }
            else
            {
                Debug.LogWarning($"No se encontró el entrenador con tag: {trainer}");
            }
        }
    }

    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
        // ⭐ REMOVIDO: El código se ejecutará automáticamente en OnSceneLoaded
        // Ya no necesitamos duplicar la lógica aquí
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
