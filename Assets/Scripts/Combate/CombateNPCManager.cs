using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombateNPCManager : MonoBehaviour
{
    DialogoManager dialogoManager = DialogoManager.instance;
    public Dialogue dialogoCombate;

    public GameObject player;
    MovimientoJugador movementScript;
    public GameObject[] NPCs;

    Pokedex pokedex;
    public List<PokeortInstance> pokeortAmigos;
    public PokeortInstance pokeortElegido;
    public int indexPokeortElegido;
    public GameObject pokeortElegidoGO;
    int cantidadJugador;
    List<PokeortInstance> pokeortsUtilizados = new List<PokeortInstance>();

    string encounteredNPCTag;
    GameObject NPC;
    Pokedex pokedexEnemigo;
    List<PokeortInstance> pokeortEnemigos;
    int indexPokeortEnemigo;
    PokeortInstance pokeortEnemigo;
    GameObject pokeortEnemigoGO;
    int cantidadEnemigo;
    List<PokeortInstance> pokeortsDerrotadosEnemigo = new List<PokeortInstance>();

    float playerPosX;
    float playerPosY;
    float playerPosZ;
    float playerRotY;

    Attack ataqueElegido;
    Attack ataqueElegidoEnemigo;

    bool ganaste = false;
    public static CombateNPCManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //recepcion de datos
        encounteredNPCTag = PlayerPrefs.GetString("EncounteredPokemon");
        playerPosX = PlayerPrefs.GetFloat("PosX");
        playerPosY = PlayerPrefs.GetFloat("PosY");
        playerPosZ = PlayerPrefs.GetFloat("PosZ");
        playerRotY = PlayerPrefs.GetFloat("RotY");

        //buscar pokeort encontrado por su tag
        NPC = NPCs.FirstOrDefault(n => n.CompareTag(encounteredNPCTag));

        if (!NPC)
        {
            Destroy(this);
        }

        UIManager.instance.combatButtons.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //CARGAR MODELOS Y DATOS DE JUGADOR Y POKEORTS:

        //posicion jugador
        Vector3 playerPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        Quaternion playerRotation = new Quaternion(0, playerRotY, 0, 0);
        player = Instantiate(player, playerPosition, playerRotation);
        GameObject.Find("Camara Principal").SetActive(false);

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
        pokeortsDerrotadosEnemigo.Add(pokeortEnemigo);
        cantidadEnemigo = pokeortEnemigos.Count;

        //cargar pokeorts en inventario
        pokedex = PokedexPlayerManager.instance.pokedex;
        pokeortAmigos = pokedex.pokeorts;
        indexPokeortElegido = 0;
        pokeortElegido = pokeortAmigos[indexPokeortElegido];
        pokeortsUtilizados.Add(pokeortElegido);
        cantidadJugador = pokeortAmigos.Count;

        //instanciar pokeort amigo
        pokeortElegidoGO = InstanciarPokeort(4f, pokeortElegido.pokemonData.PokeortPrefab, player.transform);

        //instanciar pokeort amigo
        pokeortEnemigoGO = InstanciarPokeort(10f, pokeortEnemigo.pokemonData.PokeortPrefab, player.transform);

        //frenar movimiento pokeorts
        pokeortElegidoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortEnemigoGO.GetComponent<MovimientoPokeorts>().enabled = false;

        UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido);
        UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderEnemigo, pokeortEnemigo);

        //UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UIManager.instance.combatButtons.SetActive(true);
        UIManager.instance.botonesIniciales.SetActive(true);
    }

    public bool AtaqueAmigo(GameObject botonClickeado)
    {
        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);
        UIManager.instance.EsconderAtaques();
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
            if (pokeortDerrotadoInstance == pokeortElegido)
            {
                pokeortsUtilizados.Add(pokeortDerrotadoInstance);
                PokedexUIManager.instance.MostrarEleccionPokeorts();
            }
            else
            {
                pokeortsDerrotadosEnemigo.Add(pokeortDerrotadoInstance);
                index++;
                pokeortDerrotadoInstance = pokeortEnemigos[index];
            }

            pokeortDerrotadoGO = InstanciarPokeort(distancia, pokeortDerrotadoInstance.pokemonData.PokeortPrefab, player.transform);
            pokeortDerrotadoGO.GetComponent<MovimientoPokeorts>().enabled = false;

            GameObject slider = (pokeortDerrotadoInstance == pokeortElegido) ? UIManager.instance.sliderAmigo : UIManager.instance.sliderEnemigo;
            UIManager.instance.ActualizarBarraDeVida(slider, pokeortDerrotadoInstance);
        }
        else
        {
            if (pokeortDerrotadoInstance == pokeortElegido)
            {
                DialogueLine line1 = new DialogueLine();
                line1.speakerName = "Sistema";
                line1.dialogueText = "No tienes más Pokeorts.";

                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "Has perdido la batalla.";

                dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2 };
                dialogoManager.StartDialogue(dialogoCombate);
            }
            else
            {
                DialogueLine line1 = new DialogueLine();
                line1.speakerName = "Sistema";
                line1.dialogueText = "El rival no tiene mas pokeorts.";

                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "Has ganado la batalla.";

                dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2 };
                dialogoManager.StartDialogue(dialogoCombate);

                ganaste = true;
            }

            TerminarBatalla();
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
            StartCoroutine(SecuenciaDeAtaque(
                () => AtaqueEnemigo(),
                pokeortElegido,
                pokeortElegidoGO,
                UIManager.instance.sliderAmigo,
                () => AtaqueAmigo(botonClickeado),
                pokeortEnemigo,
                pokeortEnemigoGO,
                UIManager.instance.sliderEnemigo
            ));
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            StartCoroutine(SecuenciaDeAtaque(
                () => AtaqueAmigo(botonClickeado),
                pokeortEnemigo,
                pokeortEnemigoGO,
                UIManager.instance.sliderEnemigo,
                () => AtaqueEnemigo(),
                pokeortElegido,
                pokeortElegidoGO,
                UIManager.instance.sliderAmigo
            ));
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                StartCoroutine(SecuenciaDeAtaque(
                    () => AtaqueEnemigo(),
                    pokeortElegido,
                    pokeortElegidoGO,
                    UIManager.instance.sliderAmigo,
                    () => AtaqueAmigo(botonClickeado),
                    pokeortEnemigo,
                    pokeortEnemigoGO,
                    UIManager.instance.sliderEnemigo
                ));
            }
            else
            {
                StartCoroutine(SecuenciaDeAtaque(
                    () => AtaqueAmigo(botonClickeado),
                    pokeortEnemigo,
                    pokeortEnemigoGO,
                    UIManager.instance.sliderEnemigo,
                    () => AtaqueEnemigo(),
                    pokeortElegido,
                    pokeortElegidoGO,
                    UIManager.instance.sliderAmigo
                ));
            }
        }
    }

    private IEnumerator SecuenciaDeAtaque(
        System.Func<bool> primerAtaque,
        PokeortInstance defensor1, GameObject defensor1GO, GameObject slider1,
        System.Func<bool> segundoAtaque,
        PokeortInstance defensor2, GameObject defensor2GO, GameObject slider2)
    {
        bool resultado1 = primerAtaque();
        yield return new WaitUntil(() => !dialogoManager.talking);
        UIManager.instance.ActualizarBarraDeVida(slider1, defensor1);

        if (!resultado1)
        {
            if (defensor1 == pokeortElegido)
            {
                Derrotado(4f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
            }
            else
            {
                Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
            }
            yield break;
        }

        bool resultado2 = segundoAtaque();
        yield return new WaitUntil(() => !dialogoManager.talking);
        UIManager.instance.ActualizarBarraDeVida(slider2, defensor2);

        if (!resultado2)
        {
            if (defensor2 == pokeortElegido)
            {
                Derrotado(4f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
            }
            else
            {
                Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
            }
        }
    }
    public IEnumerator EjecutarAtaqueConDialogo(System.Func<bool> ataque, PokeortInstance defensor, GameObject defensorGO, GameObject slider)
    {
        yield return new WaitUntil(() => !dialogoManager.talking);

        bool resultado = ataque();

        yield return new WaitUntil(() => !dialogoManager.talking);

        UIManager.instance.ActualizarBarraDeVida(slider, defensor);

        if (!resultado)
        {
            if (defensor == pokeortElegido)
            {
                Derrotado(4f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador);
            }
            else
            {
                Derrotado(10f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    
    public void TerminarBatalla()
    {
        if (ganaste)
        {
            foreach (PokeortInstance pokeort in pokeortsUtilizados)
            {
                int nivelInicial = pokeort.level;
                int totalXP = 0;

                foreach (PokeortInstance pokeortEnemigo in pokeortsDerrotadosEnemigo)
                {
                    int baseA = Mathf.RoundToInt(Mathf.Pow(2 * pokeortEnemigo.level + 10, 5 / 2));
                    int baseB = Mathf.RoundToInt(Mathf.Pow(pokeortEnemigo.level + pokeort.level + 10, 5 / 2));
                    int baseC = Mathf.RoundToInt(pokeortEnemigo.pokemonData.baseXP * pokeortEnemigo.level / pokeortsUtilizados.Count / 5);

                    int xp = baseC * baseA / baseB + 1;
                    Debug.Log(xp);
                    pokeort.experiencePoints += xp;
                    totalXP += xp;

                    pokeort.ChequearNivel();
                }

                DialogueLine line = new DialogueLine();
                line.speakerName = "Sistema";
                line.dialogueText = pokeort.pokemonData.pokemonName + " ha ganado " + totalXP + " puntos de experiencia.";

                if (pokeort.level > nivelInicial)
                {
                    DialogueLine line2 = new DialogueLine();
                    line2.speakerName = "Sistema";
                    line2.dialogueText = pokeort.pokemonData.pokemonName + " ha subido al nivel " + pokeort.level + "!";

                    dialogoCombate.dialogueLines = new List<DialogueLine> { line, line2 };
                }
                else
                {
                    dialogoCombate.dialogueLines = new List<DialogueLine> { line };
                }

                dialogoManager.StartDialogue(dialogoCombate);

                IEnumerator WaitXP()
                {
                    yield return new WaitUntil(() => !dialogoManager.talking);
                }

                StartCoroutine(WaitXP());
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitUntil(() => !dialogoManager.talking);
            UIManager.instance.combatButtons.SetActive(false);
            GameManager.instance.GameScene();
        }

        StartCoroutine(Wait());
    }

    public void CambiarPokeort()
    {
        DialogueLine line1 = new DialogueLine();
        line1.speakerName = "Sistema";
        line1.dialogueText = "Has cambiado de " + pokeortElegidoGO.GetComponent<PokemonManager>().pokemonTemplate.pokemonName + " a " + pokeortElegido.pokemonData.pokemonName + ".";

        dialogoCombate.dialogueLines = new List<DialogueLine> { line1 };
        dialogoManager.StartDialogue(dialogoCombate);

        Destroy(pokeortElegidoGO);
        pokeortElegidoGO = InstanciarPokeort(4f, pokeortElegido.pokemonData.PokeortPrefab, player.transform);
        UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido);
        pokeortElegidoGO.GetComponent<MovimientoPokeorts>().enabled = false;

        StartCoroutine(EjecutarAtaqueConDialogo(AtaqueEnemigo, pokeortElegido, pokeortElegidoGO, UIManager.instance.sliderAmigo));
    }
}
