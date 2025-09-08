using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombateSalvajeManager : MonoBehaviour
{
    public static CombateSalvajeManager instance;

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
    public List<PokeortInstance> pokeortAmigos;
    public PokeortInstance pokeortElegido;
    int indexPokeortElegido;
    public GameObject pokeortElegidoGO;
    int cantidadJugador;

    public GameObject pokeortEnemigoGO;
    PokemonManager pokeortEnemigoManager;
    public PokeortInstance pokeortEnemigo;

    Attack ataqueElegido;
    Attack ataqueElegidoEnemigo;

    List<PokeortInstance> PokeortsUtilizados;

    void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

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
        pokeortElegidoGO.GetComponent<EncuentroPokemon>().enabled = false;
        pokeortEnemigoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortEnemigoGO.GetComponent<EncuentroPokemon>().enabled = false;

        //UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public bool AtaqueAmigo(GameObject botonClickeado) {
        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);
        UIManager.instance.EsconderAtaques();
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo, dialogoCombate, dialogoManager);
    }

    public bool AtaqueEnemigo() {
        int random = Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido, dialogoCombate, dialogoManager);
    }

    void Derrotado(ref PokeortInstance pokeortDerrotadoInstance, ref GameObject pokeortDerrotadoGO)
    {
        DialogueLine line1 = new DialogueLine();
        line1.speakerName = "Sistema";
        line1.dialogueText = pokeortDerrotadoInstance.pokemonData.pokemonName + " ha sido derrotado.";
        dialogoCombate.dialogueLines.Add(line1);

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
                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "No tienes más Pokeorts. Has perdido la batalla.";
                dialogoCombate.dialogueLines.Add(line2);

                Debug.Log("Batalla Finalizada");
                return;
            }
        }
        else
        {
            DialogueLine line2 = new DialogueLine();
            line2.speakerName = "Sistema";
            line2.dialogueText = "Has derrotado a " + pokeortDerrotadoInstance.pokemonData.pokemonName + ".";
            dialogoCombate.dialogueLines.Add(line2);

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

    IEnumerator EjecutarAtaqueTrasDialogo(System.Func<bool> ataque, PokeortInstance pokeortAtacado, GameObject pokeortAtacadoGO)
    {
        yield return new WaitUntil(() => !dialogoManager.talking);

        Slider slider = (pokeortAtacado == pokeortElegido) ? UIManager.instance.sliderAmigo : UIManager.instance.sliderEnemigo;
        UIManager.instance.ActualizarBarraDeVida(slider, pokeortAtacado.currentHP, pokeortAtacado.maxHP);

        if (!ataque())
        {
            Derrotado(ref pokeortAtacado, ref pokeortAtacadoGO);
        }
    }

    public void CheckBattleState(GameObject botonClickeado)
    {
        if (pokeortEnemigo.currentSpeed > pokeortElegido.currentSpeed)
        {

            bool ataque = AtaqueEnemigo();
            UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido.currentHP, pokeortElegido.maxHP);

            if (!ataque)
            {
                Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                return;
            }

            StartCoroutine(EjecutarAtaqueTrasDialogo(() => AtaqueAmigo(botonClickeado), pokeortEnemigo, pokeortEnemigoGO));
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            bool ataque = AtaqueAmigo(botonClickeado);
            UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderEnemigo, pokeortEnemigo.currentHP, pokeortEnemigo.maxHP);

            if (!ataque)
            {
                Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                return;
            }

            StartCoroutine((EjecutarAtaqueTrasDialogo(() => AtaqueEnemigo(), pokeortElegido, pokeortElegidoGO)));
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                bool ataque = AtaqueEnemigo();
                UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido.currentHP, pokeortElegido.maxHP);

                if (!ataque)
                {
                    Derrotado(ref pokeortElegido, ref pokeortElegidoGO);
                    return;
                }

                StartCoroutine(EjecutarAtaqueTrasDialogo(() => AtaqueAmigo(botonClickeado), pokeortEnemigo, pokeortEnemigoGO));

            }
            else
            {
                bool ataque = AtaqueAmigo(botonClickeado);
                UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderEnemigo, pokeortEnemigo.currentHP, pokeortEnemigo.maxHP);

                if (!ataque)
                {
                    Derrotado(ref pokeortEnemigo, ref pokeortEnemigoGO);
                    return;
                }

                StartCoroutine((EjecutarAtaqueTrasDialogo(() => AtaqueEnemigo(), pokeortElegido, pokeortElegidoGO)));
            }
        }
    }

    public void TerminarBatalla()
    {
        foreach (PokeortInstance pokeort in PokeortsUtilizados)
        {
            int baseA = Mathf.RoundToInt(Mathf.Pow(2 * pokeortEnemigo.level + 10, 5 / 2));
            int baseB = Mathf.RoundToInt(Mathf.Pow(pokeortEnemigo.level + pokeort.level + 10, 5 / 2));
            int baseC = Mathf.RoundToInt(pokeortEnemigo.baseXP * pokeortEnemigo.level / PokeortsUtilizados.Count / 5);

            int xp = baseC * baseA / baseB + 1;
            pokeort.experiencePoints += xp;

            pokeort.ChequearNivel();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
