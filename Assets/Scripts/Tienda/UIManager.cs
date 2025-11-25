using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Button botonAtaque;
    public Button cancelar;
    public GameObject botonesIniciales;
    public GameObject botonesAtaque;
    public GameObject combatButtons;
    public TMP_Text PlataText;
    public GameObject sliderAmigo;
    public GameObject sliderEnemigo;
    public GameObject InventarioUI;
    public GameObject Pokedex;

    public bool enCombate = false;

    private void Awake()
    {
        if (instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        PersistentRoot.Instance.AddToRoot(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuscarTextoDinero();
    }

    private void Start()
    {
        BuscarTextoDinero();
    }

    private void BuscarTextoDinero()
    {
        GameObject dineroObj = GameObject.Find("Dinero");
        if (dineroObj != null)
        {
            PlataText = dineroObj.GetComponent<TMP_Text>();
            Debug.Log($"💰 Texto de dinero encontrado en la escena '{SceneManager.GetActiveScene().name}'.");
        }
        else
        {
            Debug.LogWarning($"⚠️ No se encontró el objeto 'Dinero' en la escena '{SceneManager.GetActiveScene().name}'.");
        }
    }
    public void CargarAtaques()
    {
        int index = 0;

        int cancelcount = 0;
        foreach (Transform child in botonesAtaque.transform)
        {
            if (child == cancelar.transform && cancelcount == 0)
            {
                cancelcount++;
                continue;
            }
            Destroy(child.gameObject);
        }

        Transform canvasTransform = FindObjectOfType<Canvas>().transform;
        botonesIniciales.SetActive(false);

        TextMeshProUGUI textoBotonAtaque = botonAtaque.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform rt = botonAtaque.GetComponent<RectTransform>();
        Vector2 posicionInicial = rt.anchoredPosition;
        Vector2 posicionActual = posicionInicial;

        if (CombateSalvajeManager.instance != null)
        {
            foreach (Attack ataque in CombateSalvajeManager.instance.pokeortElegido.equippedAttacks)
            {
                index++;
                GameObject nuevoBotonGO = Instantiate(botonAtaque.gameObject, botonesAtaque.transform);
                nuevoBotonGO.SetActive(true);
                RectTransform nuevoBotonRT = nuevoBotonGO.GetComponent<RectTransform>();
                nuevoBotonRT.anchoredPosition = posicionActual;
                posicionActual.y += 50f;
                TextMeshProUGUI nuevoTextoBoton = nuevoBotonGO.GetComponentInChildren<TextMeshProUGUI>();

                if (nuevoTextoBoton != null)
                {
                    nuevoTextoBoton.text = ataque.attackName;
                }
            }

            botonesAtaque.SetActive(true);
        }
        else
        {
            foreach (Attack ataque in CombateNPCManager.instance.pokeortElegido.equippedAttacks)
            {
                index++;
                GameObject nuevoBotonGO = Instantiate(botonAtaque.gameObject, botonesAtaque.transform);
                nuevoBotonGO.SetActive(true);
                RectTransform nuevoBotonRT = nuevoBotonGO.GetComponent<RectTransform>();
                nuevoBotonRT.anchoredPosition = posicionActual;
                posicionActual.y += 50f;
                TextMeshProUGUI nuevoTextoBoton = nuevoBotonGO.GetComponentInChildren<TextMeshProUGUI>();

                if (nuevoTextoBoton != null)
                {
                    nuevoTextoBoton.text = ataque.attackName;
                }
            }

            botonesAtaque.SetActive(true);
        }
    }

    public void EsconderAtaques()
    {
        botonesIniciales.SetActive(true);
        botonesAtaque.SetActive(false);
    }
    public void EsconderInventario()
    {
        botonesIniciales.SetActive(true);
        InventarioUI.SetActive(false);
    }
    public void EsconderCambioPokeort ()
    {
        if (CombateNPCManager.instance || CombateSalvajeManager.instance)
        {
            botonesIniciales.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Pokedex.SetActive(false);
    }

    public void DialogoCombate(Dialogue dialogo, DialogoManager dialogoManager)
    {
        dialogoManager.StartDialogue(dialogo);
    }

    public void ActualizarBarraDeVida(GameObject barraDeVida, PokeortInstance pokeort)
    {
        Slider slider = barraDeVida.GetComponentInChildren<Slider>();
        Image imagen = slider.fillRect.GetComponent<Image>();

        TextMeshProUGUI textoNombre = barraDeVida.transform.Find("name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textoHP = barraDeVida.transform.Find("hp").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textoLevel = barraDeVida.transform.Find("level").GetComponent<TextMeshProUGUI>();

        int vidaActual = pokeort.currentHP;
        int vidaMaxima = pokeort.maxHP;

        textoNombre.text = pokeort.pokemonData.pokemonName;
        textoLevel.text = $"NV. {pokeort.level}";
        textoHP.text = $"{vidaActual} / {vidaMaxima}";

        slider.value = (float)vidaActual / vidaMaxima;

        imagen.color = Color.Lerp(Color.red, Color.green, slider.value);
    }

    public void ActualizarSliderPokedex(GameObject barraDeVida, PokeortInstance pokeort)
    {
        Slider slider = barraDeVida.GetComponentInChildren<Slider>();
        Image imagen = slider.fillRect.GetComponent<Image>();
        int vidaActual = pokeort.currentHP;
        int vidaMaxima = pokeort.maxHP;
        slider.value = (float)vidaActual / vidaMaxima;
        imagen.color = Color.Lerp(Color.red, Color.green, slider.value);
    }
    public void ActualizarTextPlata(int Plata)
    {
        PlataText.text = "$" + Plata.ToString();
    }
}
