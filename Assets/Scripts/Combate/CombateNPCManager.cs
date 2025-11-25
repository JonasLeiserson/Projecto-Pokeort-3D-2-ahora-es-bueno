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
    List<PokeortInstance> pokeortsDerrotadosJugador = new List<PokeortInstance>();

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

        // Recepción de datos del encuentro
        encounteredNPCTag = PlayerPrefs.GetString("EncounteredPokemon");
        playerPosX = PlayerPrefs.GetFloat("PosX");
        playerPosY = PlayerPrefs.GetFloat("PosY");
        playerPosZ = PlayerPrefs.GetFloat("PosZ");
        playerRotY = PlayerPrefs.GetFloat("RotY");

        // Buscar el PREFAB del NPC por nombre
        NPC = NPCs.FirstOrDefault(n => n.name == encounteredNPCTag);

        if (!NPC)
        {
            Debug.LogError($"⚠️ No se encontró ningún NPC con nombre: {encounteredNPCTag}");
            Debug.LogError($"NPCs disponibles: {string.Join(", ", NPCs.Select(n => n.name))}");
            return;
        }

        UIManager.instance.combatButtons.SetActive(true);
        Debug.Log($"✅ NPC encontrado: {NPC.name}");
    }

    void Start()
    {
        //CARGAR MODELOS Y DATOS DE JUGADOR Y POKEORTS:

        //posicion jugador
        Vector3 playerPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        Quaternion playerRotation = Quaternion.Euler(0, playerRotY, 0);
        player = Instantiate(player, playerPosition, playerRotation);
        GameManager.instance.playerPosition = playerPosition;

        // Desactivar cámara principal
        GameObject mainCamera = GameObject.Find("Camara Principal");
        if (mainCamera != null)
        {
            mainCamera.SetActive(false);
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera")?.SetActive(false);
        }

        movementScript = player.GetComponent<MovimientoJugador>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        //posicion npc enemigo (adelante del jugador, más lejos)
        float distanciaNPCEnemigo = 12f;
        Vector3 direccionAdelante = -player.transform.right;
        Vector3 nuevaPosicionEnemigo = player.transform.position + (direccionAdelante * distanciaNPCEnemigo);
        nuevaPosicionEnemigo.y = player.transform.position.y;

        // ROTACIÓN NPC ENEMIGO - Debe mirar hacia el jugador
        Vector3 direccionHaciaJugador = (player.transform.position - nuevaPosicionEnemigo).normalized;
        Quaternion rotacionNPC = Quaternion.Euler(0, playerRotY - 180f, 0);
        NPC = Instantiate(NPC, nuevaPosicionEnemigo, rotacionNPC);

        NPC.transform.Find("Campo de Vision").GetComponent<VisionNPC>().enabled = false;

        //cargar pokeorts enemigos en inventario
        PokedexManagerNPC npcPokedexManager = NPC.GetComponent<PokedexManagerNPC>();
        if (npcPokedexManager != null && npcPokedexManager.pokedex != null)
        {
            pokedexEnemigo = npcPokedexManager.pokedex;
            pokeortEnemigos = pokedexEnemigo.pokeorts;

            ActualizarPokedexEnemiga();

            if (pokeortEnemigos != null && pokeortEnemigos.Count > 0)
            {
                indexPokeortEnemigo = 0;
                pokeortEnemigo = pokeortEnemigos[indexPokeortEnemigo];
                pokeortsDerrotadosEnemigo.Add(pokeortEnemigo);
                cantidadEnemigo = pokeortEnemigos.Count;
            }
            else
            {
                Debug.LogError("⚠️ El NPC no tiene Pokeorts en su pokedex!");
            }
        }
        else
        {
            Debug.LogError("⚠️ El NPC no tiene PokedexManagerNPC o su pokedex es null!");
        }

        //cargar pokeorts del jugador en inventario
        if (PokedexPlayerManager.instance != null && PokedexPlayerManager.instance.pokedex != null)
        {
            pokedex = PokedexPlayerManager.instance.pokedex;
            pokeortAmigos = pokedex.pokeorts;

            Debug.Log($"[DIAGNÓSTICO] PokedexPlayerManager cargó {pokeortAmigos?.Count ?? 0} Pokeorts.");

            if (pokeortAmigos != null && pokeortAmigos.Count > 0)
            {
                indexPokeortElegido = 0;
                pokeortElegido = pokeortAmigos[indexPokeortElegido];
                pokeortsUtilizados.Add(pokeortElegido);
                cantidadJugador = pokeortAmigos.Count;
            }
            else
            {
                Debug.LogError("⚠️ El jugador no tiene Pokeorts!");
            }
        }
        else
        {
            Debug.LogError("⚠️ PokedexPlayerManager no existe o su pokedex es null!");
        }

        // Instanciar pokeorts en posiciones relativas al jugador
        if (pokeortElegido != null && pokeortElegido.pokemonData != null)
        {
            pokeortElegidoGO = InstanciarPokeort(3f, pokeortElegido.pokemonData.PokeortPrefab, player.transform, false);

            if (pokeortElegidoGO != null)
            {
                MovimientoPokeorts movPokeort = pokeortElegidoGO.GetComponent<MovimientoPokeorts>();
                if (movPokeort != null) movPokeort.enabled = false;

                EncuentroPokemon encPokeort = pokeortElegidoGO.GetComponent<EncuentroPokemon>();
                if (encPokeort != null) encPokeort.enabled = false;
            }
        }

        if (pokeortEnemigo != null && pokeortEnemigo.pokemonData != null)
        {
            pokeortEnemigoGO = InstanciarPokeort(9f, pokeortEnemigo.pokemonData.PokeortPrefab, player.transform, true);

            if (pokeortEnemigoGO != null)
            {
                MovimientoPokeorts movPokeort = pokeortEnemigoGO.GetComponent<MovimientoPokeorts>();
                if (movPokeort != null) movPokeort.enabled = false;

                EncuentroPokemon encPokeort = pokeortEnemigoGO.GetComponent<EncuentroPokemon>();
                if (encPokeort != null) encPokeort.enabled = false;
            }
        }

        // Actualizar UI
        if (UIManager.instance != null)
        {
            if (pokeortElegido != null)
            {
                UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderAmigo, pokeortElegido);
                // ⭐ CORRECCIÓN: Cargar los ataques del Pokeort elegido en la UI
                // Asegúrate de que tu UIManager tenga este método (CargarAtaques)
                UIManager.instance.CargarAtaques();
            }

            if (pokeortEnemigo != null)
            {
                UIManager.instance.ActualizarBarraDeVida(UIManager.instance.sliderEnemigo, pokeortEnemigo);
            }

            UIManager.instance.combatButtons.SetActive(true);
            UIManager.instance.botonesIniciales.SetActive(true);
        }

        //UI Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        UIManager.instance.EsconderAtaques();
    }

    public bool AtaqueAmigo(GameObject botonClickeado)
    {
        if (botonClickeado == null || pokeortElegido == null) return false;

        TextMeshProUGUI nombreAtaque = botonClickeado.GetComponentInChildren<TextMeshProUGUI>();
        if (nombreAtaque == null) return false;

        ControlCursor.instance.MostrarCursor();

        ataqueElegido = pokeortElegido.equippedAttacks.FirstOrDefault(a => a.attackName == nombreAtaque.text);

        if (ataqueElegido == null) return false;

        UIManager.instance.EsconderAtaques();
        
        // ⭐ Pasar 'this' (CombateNPCManager) como el MonoBehaviour que ejecutará la corrutina
        Animator animAmigo = pokeortElegidoGO?.GetComponentInChildren<Animator>();
        Transform TRenemigo = pokeortEnemigoGO.GetComponent<Transform>();
        Transform TRamigo = pokeortElegidoGO.GetComponent<Transform>();
        return pokeortElegido.atacar(ataqueElegido, pokeortEnemigo, TRenemigo, TRamigo, dialogoCombate, dialogoManager, animAmigo, this);
    }

    public bool AtaqueEnemigo()
    {
        if (pokeortEnemigo == null || pokeortEnemigo.equippedAttacks == null || pokeortEnemigo.equippedAttacks.Count == 0)
        {
            return false;
        }

        int random = Random.Range(0, pokeortEnemigo.equippedAttacks.Count);
        ataqueElegidoEnemigo = pokeortEnemigo.equippedAttacks[random];
        
        UIManager.instance.EsconderAtaques();

        Animator animEnemigo = pokeortEnemigoGO?.GetComponentInChildren<Animator>();
        Transform TRamigo = pokeortEnemigoGO.GetComponent<Transform>();
        Transform TRenemigo = pokeortElegidoGO.GetComponent<Transform>();
        return pokeortEnemigo.atacar(ataqueElegidoEnemigo, pokeortElegido, TRenemigo, TRamigo, dialogoCombate, dialogoManager, animEnemigo, this);
    }

    void Derrotado(float distancia, ref int index, ref List<PokeortInstance> pokeorts, ref PokeortInstance pokeortDerrotadoInstance, ref GameObject pokeortDerrotadoGO, ref int cantidad, bool esEnemigo)
    {
        if (pokeortDerrotadoGO != null)
        {
            Destroy(pokeortDerrotadoGO);
        }

        cantidad--;
        if (cantidad > 0)
        {
            if (pokeortDerrotadoInstance == pokeortElegido)
            {
                if (!pokeortsDerrotadosJugador.Contains(pokeortDerrotadoInstance))
                {
                    pokeortsDerrotadosJugador.Add(pokeortDerrotadoInstance);
                }

                if (PokedexUIManager.instance != null)
                {
                    PokedexUIManager.instance.MostrarEleccionPokeorts();
                }
            }
            else
            {
                if (!pokeortsDerrotadosEnemigo.Contains(pokeortDerrotadoInstance))
                {
                    pokeortsDerrotadosEnemigo.Add(pokeortDerrotadoInstance);
                }
                index++;

                if (index < pokeortEnemigos.Count)
                {
                    pokeortDerrotadoInstance = pokeortEnemigos[index];
                }
            }

            if (pokeortDerrotadoInstance != null && pokeortDerrotadoInstance.pokemonData != null)
            {
                pokeortDerrotadoGO = InstanciarPokeort(distancia, pokeortDerrotadoInstance.pokemonData.PokeortPrefab, player.transform, esEnemigo);

                if (pokeortDerrotadoGO != null)
                {
                    MovimientoPokeorts movPokeort = pokeortDerrotadoGO.GetComponent<MovimientoPokeorts>();
                    if (movPokeort != null) movPokeort.enabled = false;
                }

                GameObject slider = (pokeortDerrotadoInstance == pokeortElegido) ? UIManager.instance.sliderAmigo : UIManager.instance.sliderEnemigo;
                if (UIManager.instance != null && slider != null)
                {
                    UIManager.instance.ActualizarBarraDeVida(slider, pokeortDerrotadoInstance);
                }
            }
        }
        else
        {
            // No quedan más Pokeorts
            if (pokeortDerrotadoInstance == pokeortElegido)
            {
                DialogueLine line1 = new DialogueLine();
                line1.speakerName = "Sistema";
                line1.dialogueText = "No tienes más Pokeorts.";

                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "Has perdido la batalla.";

                dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2 };

                if (dialogoManager != null)
                {
                    dialogoManager.StartDialogue(dialogoCombate);
                }
            }
            else
            {
                DialogueLine line1 = new DialogueLine();
                line1.speakerName = "Sistema";
                line1.dialogueText = "El rival no tiene más Pokeorts.";

                DialogueLine line2 = new DialogueLine();
                line2.speakerName = "Sistema";
                line2.dialogueText = "Has ganado la batalla.";

                dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2 };

                if (dialogoManager != null)
                {
                    dialogoManager.StartDialogue(dialogoCombate);
                }

                ganaste = true;
            }

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
        if (pokeortEnemigo == null || pokeortElegido == null)
        {
            Debug.LogError("⚠️ Uno de los Pokeorts es null en CheckBattleState!");
            return;
        }

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

        if (UIManager.instance != null && slider1 != null)
        {
            UIManager.instance.ActualizarBarraDeVida(slider1, defensor1);
        }

        if (!resultado1)
        {
            if (defensor1 == pokeortElegido)
            {
                Derrotado(3f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador, false);
            }
            else
            {
                Derrotado(9f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo, true);
            }
            yield break;
        }

        bool resultado2 = segundoAtaque();
        yield return new WaitUntil(() => !dialogoManager.talking);

        if (UIManager.instance != null && slider2 != null)
        {
            UIManager.instance.ActualizarBarraDeVida(slider2, defensor2);
        }

        if (!resultado2)
        {
            if (defensor2 == pokeortElegido)
            {
                Derrotado(3f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador, false);
            }
            else
            {
                Derrotado(9f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo, true);
            }
        }
    }

    public IEnumerator EjecutarAtaqueConDialogo(System.Func<bool> ataque, PokeortInstance defensor, GameObject defensorGO, GameObject slider)
    {
        yield return new WaitUntil(() => !dialogoManager.talking);

        bool resultado = ataque();

        yield return new WaitUntil(() => !dialogoManager.talking);

        if (UIManager.instance != null && slider != null)
        {
            UIManager.instance.ActualizarBarraDeVida(slider, defensor);
        }

        if (!resultado)
        {
            if (defensor == pokeortElegido)
            {
                Derrotado(3f, ref indexPokeortElegido, ref pokeortAmigos, ref pokeortElegido, ref pokeortElegidoGO, ref cantidadJugador, false);
            }
            else
            {
                Derrotado(9f, ref indexPokeortEnemigo, ref pokeortEnemigos, ref pokeortEnemigo, ref pokeortEnemigoGO, ref cantidadEnemigo, true);
            }
        }
    }

    void Update()
    {
        
    }

    public void TerminarBatalla()
    {
        pokeortElegido.currentAttack = pokeortElegido.maxAttack;
        pokeortElegido.currentSpAttack = pokeortElegido.maxSpAttack;
        pokeortElegido.currentDefense = pokeortElegido.maxDefense;
        pokeortElegido.currentSpDefense = pokeortElegido.maxSpDefense;
        pokeortElegido.currentSpeed = pokeortElegido.maxSpeed;

        if (ganaste)
        {
            int dineroGanado = 0;

            foreach (PokeortInstance pokeort in pokeortsUtilizados)
            {
                foreach (PokeortInstance pokeortEnemigo in pokeortsDerrotadosEnemigo)
                {
                    int baseA = Mathf.RoundToInt(Mathf.Pow(2 * pokeortEnemigo.level + 10, 2.5f));
                    int baseB = Mathf.RoundToInt(Mathf.Pow(pokeortEnemigo.level + pokeort.level + 10, 2.5f));
                    int baseC = Mathf.RoundToInt(pokeortEnemigo.pokemonData.baseXP * pokeortEnemigo.level / pokeortsUtilizados.Count / 5);

                    int xp = baseC * baseA / baseB + 1;
                    pokeort.experiencePoints += xp;
                    pokeort.ChequearNivel();

                    int dineroPorPokeort = Mathf.RoundToInt(pokeortEnemigo.level * 15 + pokeortEnemigo.pokemonData.baseXP * 0.3f);
                    dineroGanado += dineroPorPokeort;
                }
            }

            dineroGanado = Mathf.RoundToInt(dineroGanado * (1f + (pokeortsDerrotadosEnemigo.Count * 0.2f)));

            if (PlataManager.instance != null)
            {
                PlataManager.instance.AgregarPlata(dineroGanado);
            }

            DialogueLine line1 = new DialogueLine { speakerName = "Sistema", dialogueText = "El rival no tiene más Pokeorts." };
            DialogueLine line2 = new DialogueLine { speakerName = "Sistema", dialogueText = "¡Has ganado la batalla!" };
            DialogueLine line3 = new DialogueLine { speakerName = "Sistema", dialogueText = "Has ganado $" + dineroGanado + " por tu victoria." };

            dialogoCombate.dialogueLines = new List<DialogueLine> { line1, line2, line3 };

            if (dialogoManager != null)
            {
                dialogoManager.StartDialogue(dialogoCombate);
            }
        }

        if (GameManager.instance != null && NPC != null)
        {
            GameManager.instance.trainersDefeated.Add(NPC.tag);
        }

        IEnumerator Wait()
        {
            yield return new WaitUntil(() => !dialogoManager.talking);

            if (UIManager.instance != null)
            {
                UIManager.instance.combatButtons.SetActive(false);
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.GameScene();
            }
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

        if (!pokeortsUtilizados.Contains(pokeortElegido)) pokeortsUtilizados.Add(pokeortElegido);

        if (fueDerrotado) return;
        StartCoroutine(EjecutarAtaqueConDialogo(AtaqueEnemigo, pokeortElegido, pokeortElegidoGO, UIManager.instance.sliderAmigo));
    }

    void ActualizarPokedexEnemiga()
    {
        foreach (PokeortInstance pokeort in pokeortEnemigos)
        {
            pokeort.currentHP = pokeort.maxHP;
        }
    }
}
