using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vender : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject botonInteractuar; 
    public GameObject jugador;  
    public float distanciaActivacion = 3f; 
    
    private bool dentroRango = false;



    void Start()
    {
        jugador = GameObject.Find("JugadorCamaraMan");
        Debug.Log(jugador);
        botonInteractuar = GameObject.Find("InteractuarVendedor");
        botonInteractuar.SetActive(false);
    }

    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaActivacion && !dentroRango)
        {
            botonInteractuar.SetActive(true);
            dentroRango = true;
        }
        else if (distancia > distanciaActivacion && dentroRango)
        {
            botonInteractuar.SetActive(false);
            dentroRango = false;
        }
    }
}