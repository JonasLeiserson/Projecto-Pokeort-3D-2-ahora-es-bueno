using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Button botonAtaque;
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
        if (CombateSalvajeManager.instance != null)
        {
            if (botonesAtaque.transform.childCount >= 2)
            {
                botonesIniciales.SetActive(false);
                botonesAtaque.SetActive(true);
                return;
            }

            Transform canvasTransform = FindObjectOfType<Canvas>().transform;
            botonesIniciales.SetActive(false);

            TextMeshProUGUI textoBotonAtaque = botonAtaque.GetComponentInChildren<TextMeshProUGUI>();
            RectTransform rt = botonAtaque.GetComponent<RectTransform>();
            Vector2 posicionInicial = rt.anchoredPosition;
            Vector2 posicionActual = posicionInicial;

            foreach (Attack ataque in CombateSalvajeManager.instance.pokeortElegido.equippedAttacks)
            {
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
        }
        else
        {
            if (botonesAtaque.transform.childCount >= 2)
            {
                botonesIniciales.SetActive(false);
                botonesAtaque.SetActive(true);
                return;
            }

            Transform canvasTransform = FindObjectOfType<Canvas>().transform;
            botonesIniciales.SetActive(false);

            TextMeshProUGUI textoBotonAtaque = botonAtaque.GetComponentInChildren<TextMeshProUGUI>();
            RectTransform rt = botonAtaque.GetComponent<RectTransform>();
            Vector2 posicionInicial = rt.anchoredPosition;
            Vector2 posicionActual = posicionInicial;

            foreach (Attack ataque in CombateNPCManager.instance.pokeortElegido.equippedAttacks)
            {
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

        Debug.Log($"{slider.value} = {(float)vidaActual / vidaMaxima}");
        slider.value = (float)vidaActual / vidaMaxima;

        imagen.color = Color.Lerp(Color.red, Color.green, slider.value);


        Debug.Log("Health Percentage: " + slider.value);
    }
    public void CambioPokeort()
    {
         InventarioManager.instance.espaciosDePokeorts.gameObject.SetActive(true);
    }
}
