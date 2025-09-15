using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Button botonAtaque;
    public Button cancelar;
    public GameObject botonesIniciales;
    public GameObject botonesAtaque;
    public GameObject combatButtons;

    public GameObject sliderAmigo;
    public GameObject sliderEnemigo;

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
    }

    public void CargarAtaques()
    {
        int index = 0;

        foreach (Transform child in botonesAtaque.transform)
        {
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

                if (index == CombateSalvajeManager.instance.pokeortElegido.equippedAttacks.Count)
                {
                    GameObject nuevoCancelarGO = Instantiate(cancelar.gameObject, botonesAtaque.transform);
                    RectTransform cancelarRT = nuevoCancelarGO.GetComponent<RectTransform>();
                    cancelarRT.anchoredPosition = posicionActual;
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

                if (index == CombateNPCManager.instance.pokeortElegido.equippedAttacks.Count)
                {
                    GameObject nuevoCancelarGO = Instantiate(cancelar.gameObject, botonesAtaque.transform);
                    RectTransform cancelarRT = nuevoCancelarGO.GetComponent<RectTransform>();
                    cancelarRT.anchoredPosition = posicionActual;
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

    public void DialogoCombate(Dialogue dialogo, DialogoManager dialogoManager)
    {
        dialogoManager.StartDialogue(dialogo);
    }

    public void ActualizarBarraDeVida(GameObject barraDeVida, PokeortInstance pokeort)
    {
        Slider slider = barraDeVida.GetComponentInChildren<Slider>();
        Image imagen = slider.fillRect.GetComponent<Image>();

        TextMeshProUGUI textoVida = barraDeVida.GetComponentInChildren<TextMeshProUGUI>();
        int vidaActual = pokeort.currentHP;
        int vidaMaxima = pokeort.maxHP;
        textoVida.text = pokeort.pokemonData.pokemonName;

        slider.value = (float)vidaActual / vidaMaxima;

        imagen.color = Color.Lerp(Color.red, Color.green, slider.value);
    }
}
