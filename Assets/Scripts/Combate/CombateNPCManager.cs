using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombateNPCManager : MonoBehaviour
{
    public UIManager uiManager;
    public Button botonAtaque;
    public GameObject botonesAtaque;
    public GameObject botonesIniciales;

    public DialogoManager dialogoManager;
    public Dialogue dialogoCombate;

    public GameObject player;
    MovimientoJugador movementScript;
    public GameObject[] NPCs;

    Pokedex pokedex;
    List<PokeortInstance> pokeortAmigos;
    PokeortInstance pokeortElegido;
    int indexPokeortElegido;
    GameObject pokeortElegidoGO;
    int cantidadJugador;

    string encounteredNPCTag;
    GameObject NPC;
    Pokedex pokedexEnemigo;
    List<PokeortInstance> pokeortEnemigos;
    int indexPokeortEnemigo;
    PokeortInstance pokeortEnemigo;
    GameObject pokeortEnemigoGO;
    int cantidadEnemigo;

    float playerPosX;
    float playerPosY;
    float playerPosZ;
    float playerRotY;

    Attack ataqueElegido;
    Attack ataqueElegidoEnemigo;

    void Awake()
    {
        //recepcion de datos
        encounteredNPCTag = PlayerPrefs.GetString("EncounteredPokemon");
        playerPosX = PlayerPrefs.GetFloat("PosX");
        playerPosY = PlayerPrefs.GetFloat("PosY");
        playerPosZ = PlayerPrefs.GetFloat("PosZ");
        playerRotY = PlayerPrefs.GetFloat("RotY");

        //buscar pokeort encontrado por su tag
        NPC = NPCs.FirstOrDefault(n => n.CompareTag(encounteredNPCTag));
    }

    // Start is called before the first frame update
    void Start()
    {
        //CARGAR MODELOS Y DATOS DE JUGADOR Y POKEORTS:

        //posicion jugador
        Vector3 playerPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        Quaternion playerRotation = new Quaternion(0, playerRotY, 0, 0);
        player = Instantiate(player, playerPosition, playerRotation);
        player.tag = "Untagged";
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);

        GameObject cameraPositioner = GameObject.Find("CameraPositioner");
        Transform cameraPositionerTransform = cameraPositioner.transform;
        cameraPositionerTransform.position = playerPosition;
        cameraPositionerTransform.rotation = playerRotation;

        movementScript = player.GetComponent<MovimientoJugador>();
        movementScript.enabled = false;

        //posicion npc enemigo (diagonal derecha)
        float distanciaNPCEnemigo = 12f;
        Vector3 direccionDiagonal = player.transform.forward + player.transform.right;
        Vector3 direccionNormalizada = direccionDiagonal.normalized;
        Vector3 nuevaPosicionEnemigo = player.transform.position + (direccionNormalizada * distanciaNPCEnemigo);
        nuevaPosicionEnemigo.y = player.transform.position.y;
        NPC = Instantiate(NPC, nuevaPosicionEnemigo, Quaternion.identity);


        //cargar pokeorts enemigos en inventario
        pokedexEnemigo = NPC.GetComponent<PokedexManagerNPC>().pokedex;
        pokeortEnemigos = pokedexEnemigo.pokeorts;
        indexPokeortEnemigo = 0;
        pokeortEnemigo = pokeortEnemigos[indexPokeortEnemigo];
        cantidadEnemigo = pokeortEnemigos.Count;

        //cargar pokeorts en inventario
        pokedex = player.GetComponent<PokedexManagerNPC>().pokedex;
        pokeortAmigos = pokedex.pokeorts;
        indexPokeortElegido = 0;
        pokeortElegido = pokeortAmigos[indexPokeortElegido];
        cantidadJugador = pokeortAmigos.Count;

        //instanciar pokeort amigo
        pokeortElegidoGO = InstanciarPokeort(2f, pokeortElegido.pokemonData.PokeortPrefab, player.transform);

        //instanciar pokeort amigo
        pokeortEnemigoGO = InstanciarPokeort(10f, pokeortEnemigo.pokemonData.PokeortPrefab, player.transform);

        //frenar movimiento pokeorts
        pokeortElegidoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortEnemigoGO.GetComponent<MovimientoPokeorts>().enabled = false;

        //UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void cargarAtaquesUI() => uiManager.CargarAtaques(); 
 
    public bool AtaqueAmigo(GameObject botonClickeado)
    {
        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);
        uiManager.EsconderAtaques();
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo, dialogoCombate, dialogoManager);
    }

    public bool AtaqueEnemigo()
    {
        int random = Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido, dialogoCombate, dialogoManager);
    }

    void Derrotado(float distancia, ref int index, ref List<PokeortInstance> pokeorts, ref PokeortInstance pokeortDerrotadoInstance, ref GameObject pokeortDerrotadoGO, ref int cantidad)
    {
        Destroy(pokeortDerrotadoGO);

        cantidad--;
        if (cantidad > 0)
        {
            index++;
            pokeortDerrotadoInstance = pokeortEnemigos[index];
            pokeortDerrotadoGO = InstanciarPokeort(distancia, pokeortDerrotadoInstance.pokemonData.PokeortPrefab, player.transform);
            pokeortDerrotadoGO.GetComponent<MovimientoPokeorts>().enabled = false;
    }
        else
        {
            Debug.Log("Batalla Finalizada");
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

    IEnumerator EjecutarAtaqueTrasDialogo(System.Func<bool> ataque, PokeortInstance pokeortAtacado, GameObject pokeortAtacadoGO)
    {
        yield return new WaitUntil(() => !dialogoManager.talking);

        if (!ataque())
        {
            if (pokeortAtacado == pokeortElegido)
            {
                Derrotado(2f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
            } 
            else
            {
                Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
            }
        }
    }

    public void CheckBattleState(GameObject botonClickeado)
    {
        if (pokeortEnemigo.currentSpeed > pokeortElegido.currentSpeed)
        {
            if (!AtaqueEnemigo())
            {
                Derrotado(2f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
                return;
            }

            StartCoroutine(EjecutarAtaqueTrasDialogo(() => AtaqueAmigo(botonClickeado), pokeortEnemigo, pokeortEnemigoGO));
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            if (!AtaqueAmigo(botonClickeado))
            {
                Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
                return;
            }

            StartCoroutine((EjecutarAtaqueTrasDialogo(() => AtaqueEnemigo(), pokeortElegido, pokeortElegidoGO)));
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                if (!AtaqueEnemigo())
                {
                    Derrotado(2f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
                    return;
                }

                StartCoroutine(EjecutarAtaqueTrasDialogo(() => AtaqueAmigo(botonClickeado), pokeortEnemigo, pokeortEnemigoGO));

            }
            else
            {
                if (!AtaqueAmigo(botonClickeado))
                {
                    Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
                    return;
                }

                StartCoroutine((EjecutarAtaqueTrasDialogo(() => AtaqueEnemigo(), pokeortElegido, pokeortElegidoGO)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
