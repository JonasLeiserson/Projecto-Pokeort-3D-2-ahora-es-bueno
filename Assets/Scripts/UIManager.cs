using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public void CargarAtaques(List<Attack> ataques, Button botonAtaque, GameObject botonesIniciales)
    {
        Transform canvasTransform = FindObjectOfType<Canvas>().transform;
        botonesIniciales.SetActive(false);

        TextMeshProUGUI textoBotonAtaque = botonAtaque.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform rt = botonAtaque.GetComponent<RectTransform>();
        Vector2 posicionInicial = rt.anchoredPosition;
        Vector2 posicionActual = posicionInicial;

        foreach (Attack ataque in ataques)
        {
            if (!botonAtaque.gameObject.activeSelf)
            {
                botonAtaque.gameObject.SetActive(true);
                textoBotonAtaque.text = ataque.attackName;
            }
            else
            {
                posicionActual.y += 90f;
                GameObject nuevoBotonGO = Instantiate(botonAtaque.gameObject, canvasTransform);
                RectTransform nuevoBotonRT = nuevoBotonGO.GetComponent<RectTransform>();
                nuevoBotonRT.anchoredPosition = posicionActual;
                TextMeshProUGUI nuevoTextoBoton = nuevoBotonGO.GetComponentInChildren<TextMeshProUGUI>();
                if (nuevoTextoBoton != null)
                {
                    nuevoTextoBoton.text = ataque.attackName;
                }
            }
        }
    }
}
