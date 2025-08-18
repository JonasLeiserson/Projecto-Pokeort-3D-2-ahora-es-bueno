using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public void CargarAtaques(List<Attack> ataques, Button botonAtaque, GameObject botonesAtaque, GameObject botonesIniciales)
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

        foreach (Attack ataque in ataques)
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
    
    public void EsconderAtaques(GameObject botonesIniciales, GameObject botonesAtaque)
    {
        botonesIniciales.SetActive(true);
        botonesAtaque.SetActive(false);
    }

    public void DialogoCombate(Dialogue dialogo, DialogoManager dialogoManager)
    {
        dialogoManager.StartDialogue(dialogo);
    }
}
