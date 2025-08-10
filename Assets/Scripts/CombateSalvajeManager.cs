using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombateSalvajeManager : MonoBehaviour
{
    public UIManager uiManager;
    public Button botonAtaque;
    public TextMeshProUGUI textoBotonAtaque;
    public Button botonAtacar;

    public GameObject player;
    public GameObject[] pokeorts;
    GameObject encontrado;

    float playerPosX;
    float playerPosY;
    float playerPosZ;
    float playerRotY;

    string encounteredPokemonTag;
    MovimientoJugador movementScript;

    Pokedex pokedex;
    List<PokeortInstance> pokeortAmigos;
    PokeortInstance pokeortElegido;

    GameObject pokeortEnemigoInstance;
    PokemonManager pokeortEnemigoManager;
    PokeortInstance pokeortEnemigo;

    Attack ataqueElegido;
    Attack ataqueElegidoEnemigo;

    void Awake() {
        //recepcion de datos
        encounteredPokemonTag = PlayerPrefs.GetString("EncounteredPokemon");
        playerPosX = PlayerPrefs.GetFloat("PosX");
        playerPosY = PlayerPrefs.GetFloat("PosY");
        playerPosZ = PlayerPrefs.GetFloat("PosZ");
        playerRotY = PlayerPrefs.GetFloat("RotY");

        //buscar pokeort encontrado por su tag
        encontrado = pokeorts.FirstOrDefault(p => p.CompareTag(encounteredPokemonTag));
    }

    // Start is called before the first frame update
    void Start()
    {
        //CARGAR MODELOS Y DATOS DE JUGADOR Y POKEORTS:

        //posicion jugador
        Vector3 playerPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        Quaternion playerRotation = new Quaternion(0, playerRotY, 0, 0);
        player = Instantiate(player, playerPosition, playerRotation);
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);

        GameObject cameraPositioner = GameObject.Find("CameraPositioner");
        Transform cameraPositionerTransform = cameraPositioner.transform;
        cameraPositionerTransform.position = playerPosition;
        cameraPositionerTransform.rotation = playerRotation;
        

        movementScript = player.GetComponent<MovimientoJugador>();
        movementScript.enabled = false;

        //posicion pokeort enemigo (diagonal derecha)
        float distanciaEnemigo = 10f;
        Vector3 direccionDiagonal = player.transform.forward + player.transform.right;
        Vector3 direccionNormalizada = direccionDiagonal.normalized;
        Vector3 nuevaPosicionEnemigo = player.transform.position + (direccionNormalizada * distanciaEnemigo);
        nuevaPosicionEnemigo.y = player.transform.position.y;

        //instanciar pokeort encontrado
        pokeortEnemigoInstance = Instantiate(encontrado, nuevaPosicionEnemigo, Quaternion.identity);
        pokeortEnemigoManager = pokeortEnemigoInstance.GetComponent<PokemonManager>();
        pokeortEnemigo = pokeortEnemigoManager.currentPokemonInstance;

        //cargar pokeorts en inventario
        pokedex = player.GetComponent<PokedexManager>().pokedex;
        pokeortAmigos = pokedex.pokeorts;
        pokeortElegido = pokeortAmigos.First();

        //posicion pokeort amigo
        float distanciaAmigo = 2f;
        Vector3 nuevaPosicionAmigo = player.transform.position + (direccionNormalizada * distanciaAmigo);
        nuevaPosicionAmigo.y = player.transform.position.y;

        //instanciar pokeort amigo
        Instantiate(pokeortElegido.pokemonData.PokeortPrefab, nuevaPosicionAmigo, Quaternion.identity);

        //UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void cargarAtaquesUI()
    {
        uiManager.CargarAtaques(pokeortElegido.equippedAttacks, botonAtaque, textoBotonAtaque, botonAtacar);
    }

    public bool AtaqueAmigo(GameObject botonClickeado) {
        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo);
    }

    public bool AtaqueEnemigo() {
        int random = Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido);
    }

    public void CheckBattleState(GameObject botonClickeado)
    {
        if (pokeortEnemigo.currentSpeed > pokeortElegido.currentSpeed)
        {
            if (!AtaqueEnemigo()) return;
            AtaqueAmigo(botonClickeado);
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            if (!AtaqueAmigo(botonClickeado)) return;
            AtaqueEnemigo();
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                if (!AtaqueEnemigo()) return;
                AtaqueAmigo(botonClickeado);
            }
            else
            {
                if (!AtaqueAmigo(botonClickeado)) return;
                AtaqueEnemigo();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
