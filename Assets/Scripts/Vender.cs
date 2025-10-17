using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vender : MonoBehaviour
{
    public GameObject botonInteractuar;
    public GameObject jugador;
    public float distanciaActivacion = 6f;
    private bool dentroRango = false;

    void Start()
    {
        jugador = GameObject.Find("JugadorCamaraMan");
        botonInteractuar.SetActive(false);
    }

    void Update()
    {
        if (jugador == null || botonInteractuar == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.transform.position);

        if (distancia <= distanciaActivacion)
        {
            if (!dentroRango)
            {
                botonInteractuar.SetActive(true);
                dentroRango = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                CargarTienda();
            }
        }
        else
        {
            if (dentroRango)
            {
                botonInteractuar.SetActive(false);
                dentroRango = false;
            }
        }

    }
    public void CargarTienda()
    {
        TiendaUiGenerador.instance.MostrarCanvas();
        jugador.GetComponent<MovimientoJugador>();
    }
}
