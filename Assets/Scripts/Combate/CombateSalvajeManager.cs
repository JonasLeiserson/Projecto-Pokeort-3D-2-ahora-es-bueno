using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombateSalvajeManager : MonoBehaviour
{
    public static CombateSalvajeManager instance;

    readonly DialogoManager dialogoManager = DialogoManager.instance;
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
    public int indexPokeortElegido;
    public GameObject pokeortElegidoGO;
    int cantidadJugador;

    public GameObject pokeortEnemigoGO;
    PokemonManager pokeortEnemigoManager;
    public PokeortInstance pokeortEnemigo;

    Attack ataqueElegido;
    Attack ataqueElegidoEnemigo;

    List<PokeortInstance> PokeortsUtilizados = new List<PokeortInstance>();
    bool ganaste = false;

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
        encounteredPokemonTag = PlayerPrefs.GetString("EncounteredPokemon");
        playerPosX = PlayerPrefs.GetFloat("PosX");
        playerPosY = PlayerPrefs.GetFloat("PosY");
        playerPosZ = PlayerPrefs.GetFloat("PosZ");
        playerRotY = PlayerPrefs.GetFloat("RotY");

        //buscar pokeort encontrado por su tag
        encontrado = pokeorts.FirstOrDefault(p => p.CompareTag(encounteredPokemonTag));

        if (!encontrado)
        {
            Destroy(this);
        }

        UIManager.instance.combatButtons.SetActive(true);
    }

    void Start()
    {
        //CARGAR MODELOS Y DATOS DE JUGADOR Y POKEORTS:

        //posicion jugador
        Vector3 playerPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        Quaternion playerRotation = Quaternion.Euler(0, playerRotY, 0);
        player = Instantiate(player, playerPosition, playerRotation);
        GameManager.instance.playerPosition = playerPosition;
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);

        movementScript = player.GetComponent<MovimientoJugador>();
        movementScript.enabled = false;

        //cargar pokeorts en inventario
        pokedex = PokedexPlayerManager.instance.pokedex;
        pokeortAmigos = pokedex.pokeorts;
        indexPokeortElegido = 0;
        pokeortElegido = pokeortAmigos[indexPokeortElegido];
        PokeortsUtilizados.Add(pokeortElegido);
        cantidadJugador = pokeortAmigos.Count;

        //instanciar pokeort amigo (adelante del jugador)
        pokeortElegidoGO = InstanciarPokeort(4f, pokeortElegido.pokemonData.PokeortPrefab, player.transform, false);

        //instanciar y cargar pokeort enemigo (más lejos, mirando al jugador)
        Debug.Log(encontrado.name);
        pokeortEnemigoGO = InstanciarPokeort(12f, encontrado, player.transform, true);
        pokeortEnemigoManager = pokeortEnemigoGO.GetComponent<PokemonManager>();
        pokeortEnemigo = pokeortEnemigoManager.currentPokemonInstance;

        //cancelar movimiento pokeorts
        pokeortElegidoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortElegidoGO.GetComponent<EncuentroPokemon>().enabled = false;
        pokeortEnemigoGO.GetComponent<MovimientoPokeorts>().enabled = false;
        pokeortEnemigoGO.GetComponent<EncuentroPokemon>().enabled = false;

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
        Animator animAmigo = pokeortElegidoGO?.GetComponentInChildren<Animator>();
        Transform TRenemigo = pokeortEnemigoGO.GetComponent<Transform>();
        Transform TRamigo = pokeortElegidoGO.GetComponent<Transform>();
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo, TRenemigo, TRamigo, dialogoCombate, dialogoManager, animAmigo, this);
    }

    public bool AtaqueEnemigo()
    {
        int random = UnityEngine.Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        UIManager.instance.EsconderAtaques();
        Animator animEnemigo = pokeortEnemigoGO?.GetComponentInChildren<Animator>();
        Transform TRamigo = pokeortEnemigoGO.GetComponent<Transform>();
        Transform TRenemigo = pokeortElegidoGO.GetComponent<Transform>();
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido, TRenemigo, TRamigo, dialogoCombate, dialogoManager, animEnemigo, this);
    }

    void Derrotado(ref PokeortInstance pokeortDerrotadoInstance, ref GameObject pokeortDerrotadoGO)
    {
        dialogoCombate.dialogueLines.Clear();

        DialogueLine line1 = new DialogueLine();
        line1.speakerName = "Sistema";
        line1.dialogueText = pokeortDerrotadoInstance.pokemonData.pokemonName + " ha sido derrotado.";
        dialogoCombate.dialogueLines.Add(line1);

        if (pokeortDerrotadoInstance == pokeortElegido)
        {
            cantidadJugador--;
            if (cantidadJugador > 0)
            {
                if (pokeortDerrotadoInstance == pokeortElegido)
                {
                    if (PokedexUIManager.instance != null)
                    {
                        PokedexUIManager.instance.MostrarEleccionPokeorts();
                    }
                }
            }
            else
            {
                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "No tienes más Pokeorts. Has perdido la batalla.";
                dialogoCombate.dialogueLines.Add(line2);

                dialogoManager.StartDialogue(dialogoCombate);

                TerminarBatalla();
                return;
            }
        }
        else
        {
            DialogueLine line2 = new DialogueLine();
            line2.speakerName = "Sistema";
            line2.dialogueText = "Has derrotado a " + pokeortDerrotadoInstance.pokemonData.pokemonName + ".";
            dialogoCombate.dialogueLines.Add(line2);

            dialogoManager.StartDialogue(dialogoCombate);

            ganaste = true;
            TerminarBatalla();
            return;
        }
    }

    GameObject InstanciarPokeort(float distancia, GameObject prefab, Transform posicionBase, bool esEnemigo = false)
    {
        if (prefab == null || posicionBase == null)
        {
            Debug.LogError("⚠️ Prefab o posicionBase es null en InstanciarPokeort!");
            return null;
        }

        Vector3 direccionAdelante = -posicionBase.right;
        Vector3 nuevaPosicion = posicionBase.position + (direccionAdelante * distancia);
        nuevaPosicion.y = posicionBase.position.y;

        GameObject pokeortGO = Instantiate(prefab, nuevaPosicion, Quaternion.identity);
        pokeortGO.GetComponent<PokemonManager>().Init(PlayerPrefs.GetInt("Level"));

        if (esEnemigo)
        {
            Vector3 posicionAmigoEstimada = posicionBase.position + (direccionAdelante * 4f);
            pokeortGO.transform.rotation = Quaternion.Euler(0, player.transform.eulerAngles.y - 180, 0);
        }
        else
        {
            Vector3 posicionEnemigoEstimada = posicionBase.position + (direccionAdelante * 10f);
            pokeortGO.transform.rotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        }

        if (pokeortGO == null)
        {
            Debug.LogError("⚠️ No se pudo instanciar el Pokeort!");
            return null;
        }

        return pokeortGO;
    }

    public void CheckBattleState(GameObject botonClickeado)
    {
        if (pokeortEnemigo.currentSpeed > pokeortElegido.currentSpeed)
        {
            // Enemigo ataca primero
            StartCoroutine(SecuenciaDeAtaque(
                () => AtaqueEnemigo(),
                pokeortElegido,
                pokeortElegidoGO,
                () => AtaqueAmigo(botonClickeado),
                pokeortEnemigo,
                pokeortEnemigoGO
            ));
        }
        else if (pokeortEnemigo.currentSpeed < pokeortElegido.currentSpeed)
        {
            // Amigo ataca primero
            StartCoroutine(SecuenciaDeAtaque(
                () => AtaqueAmigo(botonClickeado),
                pokeortEnemigo,
                pokeortEnemigoGO,
                () => AtaqueEnemigo(),
                pokeortElegido,
                pokeortElegidoGO
            ));
        }
        else
        {
            // Velocidades iguales → aleatorio
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
            {
                StartCoroutine(SecuenciaDeAtaque(
                    () => AtaqueEnemigo(),
                    pokeortElegido,
                    pokeortElegidoGO,
                    () => AtaqueAmigo(botonClickeado),
                    pokeortEnemigo,
                    pokeortEnemigoGO
                ));
            }
            else
            {
                StartCoroutine(SecuenciaDeAtaque(
                    () => AtaqueAmigo(botonClickeado),
                    pokeortEnemigo,
                    pokeortEnemigoGO,
                    () => AtaqueEnemigo(),
                    pokeortElegido,
                    pokeortElegidoGO
                ));
            }
        }
    }

    private IEnumerator SecuenciaDeAtaque(
        System.Func<bool> ataquePrimero, PokeortInstance defensorPrimero, GameObject defensorPrimeroGO,
        System.Func<bool> ataqueSegundo, PokeortInstance defensorSegundo, GameObject defensorSegundoGO)
    {
        bool ataque1 = ataquePrimero();

        yield return new WaitUntil(() => !dialogoManager.talking);

        GameObject slider1 = (defensorPrimero == pokeortElegido)
            ? UIManager.instance.sliderAmigo
            : UIManager.instance.sliderEnemigo;
        UIManager.instance.ActualizarBarraDeVida(slider1, defensorPrimero);

        if (!ataque1)
        {
            Derrotado(ref defensorPrimero, ref defensorPrimeroGO);
            yield break;
        }

        bool ataque2 = ataqueSegundo();

        yield return new WaitUntil(() => !dialogoManager.talking);

        GameObject slider2 = (defensorSegundo == pokeortElegido)
            ? UIManager.instance.sliderAmigo
            : UIManager.instance.sliderEnemigo;
        UIManager.instance.ActualizarBarraDeVida(slider2, defensorSegundo);

        if (!ataque2)
        {
            Derrotado(ref defensorSegundo, ref defensorSegundoGO);
        }
    }

    public IEnumerator SecuenciaDeAtaqueSimple(System.Func<bool> ataque, PokeortInstance defensor, GameObject defensorGO)
    {
        yield return new WaitUntil(() => !dialogoManager.talking);

        bool ataqueExitoso = ataque();

        yield return new WaitUntil(() => !dialogoManager.talking);

        GameObject slider = (defensor == pokeortElegido)
            ? UIManager.instance.sliderAmigo
            : UIManager.instance.sliderEnemigo;
        UIManager.instance.ActualizarBarraDeVida(slider, defensor);

        if (!ataqueExitoso)
        {
            Derrotado(ref defensor, ref defensorGO);
        }
    }

    public void TerminarBatalla()
    {
        if (ganaste)
        {
            foreach (PokeortInstance pokeort in PokeortsUtilizados)
            {
                int nivelInicial = pokeort.level;

                int baseA = Mathf.RoundToInt(Mathf.Pow(2 * pokeortEnemigo.level + 10, 5 / 2));
                int baseB = Mathf.RoundToInt(Mathf.Pow(pokeortEnemigo.level + pokeort.level + 10, 5 / 2));
                int baseC = Mathf.RoundToInt(pokeortEnemigo.pokemonData.baseXP * pokeortEnemigo.level / PokeortsUtilizados.Count / 5);

                int xp = baseC * baseA / baseB + 1;
                pokeort.experiencePoints += xp;

                pokeort.ChequearNivel();

                DialogueLine line = new DialogueLine();
                line.speakerName = "Sistema";
                line.dialogueText = pokeort.pokemonData.pokemonName + " ha ganado " + xp + " puntos de experiencia.";

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

    public void CambiarPokeort(bool fueDerrotado)
    {
        if (pokeortElegidoGO == null || pokeortElegido == null) return;
        PokemonManager pokemonManager = pokeortElegidoGO.GetComponent<PokemonManager>();
        string nombreAnterior = (pokemonManager != null && pokemonManager.pokemonTemplate != null)
            ? pokemonManager.pokemonTemplate.pokemonName
            : "Pokeort anterior";

        DialogueLine line1 = new DialogueLine();
        line1.speakerName = "Sistema";
        line1.dialogueText = "Has cambiado de " + nombreAnterior + " a " + pokeortElegido.pokemonData.pokemonName + ".";

        dialogoCombate.dialogueLines = new List<DialogueLine> { line1 };

        if (dialogoManager != null)
        {
            dialogoManager.StartDialogue(dialogoCombate);
        }

        Destroy(pokeortElegidoGO);
        pokeortElegidoGO = InstanciarPokeort(4f, pokeortElegido.pokemonData.PokeortPrefab, player.transform, false);


        if (UIManager.instance != null)
        {
            UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido);
            // ⭐ CORRECCIÓN: Cargar los ataques del nuevo Pokeort
            UIManager.instance.CargarAtaques();
        }

        if (pokeortElegidoGO != null)
        {
            MovimientoPokeorts movPokeort = pokeortElegidoGO.GetComponent<MovimientoPokeorts>();
            if (movPokeort != null) movPokeort.enabled = false;
        }

        if (!PokeortsUtilizados.Contains(pokeortElegido)) PokeortsUtilizados.Add(pokeortElegido);

        if (fueDerrotado) return;
        StartCoroutine(SecuenciaDeAtaqueSimple(AtaqueEnemigo, pokeortElegido, pokeortElegidoGO));
    }

    public void UsarPokebola(Item item)
    {
        InventarioManager.instance.EsconderInventario();

        if (Pokedex.MAX_POKEMONS <= pokedex.pokeorts.Count)
        {
            DialogueLine line1 = new DialogueLine();
            line1.speakerName = "Sistema";
            line1.dialogueText = "No puedes capturar más pokeorts. Tu pokedex está llena.";
            dialogoCombate.dialogueLines = new List<DialogueLine> { line1 };
            dialogoManager.StartDialogue(dialogoCombate);
            return;
        }

        float hpFrac = pokeortEnemigo.currentHP / pokeortEnemigo.maxHP;
        float ratio = item.ValorDeUso;

        float P = 0.05f + (1f - hpFrac) * 0.70f + (ratio - 1f) * 0.15f;

        float random = UnityEngine.Random.Range(0f, 1f);
        if (P > random)
        {
            pokedex.pokeorts.Add(pokeortEnemigo);
            Destroy(pokeortEnemigoGO);

            DialogueLine line1 = new DialogueLine();
            line1.speakerName = "Sistema";
            line1.dialogueText = "¡Has capturado a " + pokeortEnemigo.pokemonData.pokemonName + "!";

            DialogueLine line2 = new DialogueLine();
            line2.speakerName = "Sistema";
            line2.dialogueText = "¡Felicidades! Batalla finalizada";

            dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2 };
            dialogoManager.StartDialogue(dialogoCombate);

            ganaste = true;

            IEnumerator Wait()
            {
                yield return new WaitUntil(() => !dialogoManager.talking);
                TerminarBatalla();
            }

            StartCoroutine(Wait());
        }
        else
        {
            DialogueLine line1 = new DialogueLine();
            line1.speakerName = "Sistema";
            line1.dialogueText = pokeortEnemigo.pokemonData.pokemonName + " ha escapado de la pokeortbola.";

            dialogoCombate.dialogueLines = new List<DialogueLine> { line1 };
            dialogoManager.StartDialogue(dialogoCombate);

            StartCoroutine(SecuenciaDeAtaqueSimple(AtaqueEnemigo, pokeortElegido, pokeortElegidoGO));
        }
    }

    public void HuirCombate(bool huyo)
    {
        if (huyo)
        {
            TerminarBatalla();
        }
        else
        {
            StartCoroutine(SecuenciaDeAtaqueSimple(AtaqueEnemigo, pokeortElegido, pokeortElegidoGO));
        }

    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            && Input.GetKeyDown(KeyCode.A))
        {
            pokedex.pokeorts.Add(pokeortEnemigo);
            Debug.Log(pokeortEnemigo.pokemonData.pokemonName + " añadido a la pokedex.");
        }
    }
}