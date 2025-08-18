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
    public GameObject botonesIniciales;
    public GameObject botonesAtaque;

    public DialogoManager dialogoManager;
    public Dialogue dialogoCombate;

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
    int indexPokeortElegido;
    GameObject pokeortElegidoGO;
    int cantidadJugador;

    GameObject pokeortEnemigoGO;
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

        //instanciar y cargar pokeort encontrado
        pokeortEnemigoGO = InstanciarPokeort(10f, encontrado, player.transform);
        pokeortEnemigoManager = pokeortEnemigoGO.GetComponent<PokemonManager>();
        pokeortEnemigo = pokeortEnemigoManager.currentPokemonInstance;

        //cargar pokeorts en inventario
        pokedex = player.GetComponent<PokedexManager>().pokedex;
        pokeortAmigos = pokedex.pokeorts;
        indexPokeortElegido = 0;
        pokeortElegido = pokeortAmigos[indexPokeortElegido];
        cantidadJugador = pokeortAmigos.Count;

        //posicion pokeort amigo
        float distanciaAmigo = 2f;
        Vector3 nuevaPosicionAmigo = player.transform.position + (direccionNormalizada * distanciaAmigo);
        nuevaPosicionAmigo.y = player.transform.position.y;

        //instanciar pokeort amigo
        pokeortElegidoGO = InstanciarPokeort(2f, pokeortElegido.pokemonData.PokeortPrefab, player.transform);

        //cancelar movimiento pokeorts
        pokeortElegidoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortEnemigoGO.GetComponent<MovimientoPokeorts>().enabled = false;

        //UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void cargarAtaquesUI() => uiManager.CargarAtaques(pokeortElegido.equippedAttacks, botonAtaque, botonesAtaque, botonesIniciales);

    public bool AtaqueAmigo(GameObject botonClickeado) {
        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);
        uiManager.EsconderAtaques(botonesIniciales, botonesAtaque);
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo, dialogoCombate, dialogoManager);
    }

    public bool AtaqueEnemigo() {
        int random = Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido, dialogoCombate, dialogoManager);
    }

    void Derrotado(ref PokeortInstance pokeortDerrotadoInstance, ref GameObject pokeortDerrotadoGO)
    {
        Destroy(pokeortDerrotadoGO);

        if (pokeortDerrotadoInstance == pokeortElegido)
        {
            cantidadJugador--;
            if (cantidadJugador > 0)
            {
                indexPokeortElegido++;
                pokeortDerrotadoInstance = pokeortAmigos[indexPokeortElegido];
                pokeortDerrotadoGO = InstanciarPokeort(2f, pokeortDerrotadoInstance.pokemonData.PokeortPrefab, player.transform);
                pokeortDerrotadoGO.GetComponent<MovimientoPokeorts>().enabled = false;
            }
            else
            {
                Debug.Log("Batalla Finalizada");
                return;
            }
        }
        else
        {
            Debug.Log("Batalla Finalizada");
            return;
        }
    }

    GameObject InstanciarPokeort(float distancia, GameObject prefab, Transform posicionBase)
    {
        Vector3 direccionDiagonal = posicionBase.forward + posicionBase.right;
        Vector3 direccionNormalizada = direccionDiagonal.normalized;
        Vector3 nuevaPosicion = posicionBase.position + (direccionNormalizada * distancia);
        nuevaPosicion.y = posicionBase.position.y;
        return Instantiate(prefab, nuevaPosicion, Quaternion.identity);
    }

    public void CheckBattleState(GameObject botonClickeado)
    {
        if (pokeortEnemigo.currentSpeed > pokeortElegido.currentSpeed)
        {
            if (!AtaqueEnemigo())
            {
                Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                return;
            }

            if (!AtaqueAmigo(botonClickeado))
            {
                Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                return;
            }
            AtaqueEnemigo();
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            if (!AtaqueAmigo(botonClickeado))
            {
                Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                return;
            }

            if (!AtaqueEnemigo())
            {
                Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                return;
            }
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                if (!AtaqueEnemigo())
                {
                    Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                    return;
                }

                if (!AtaqueAmigo(botonClickeado))
                {
                    Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                    return;
                }
                AtaqueEnemigo();
            }
            else
            {
                if (!AtaqueAmigo(botonClickeado))
                {
                    Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                    return;
                }

                if (!AtaqueEnemigo())
                {
                    Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                    return;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
