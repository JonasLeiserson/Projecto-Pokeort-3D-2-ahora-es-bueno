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
    public enum opcion{
        CargarTienda,
        CargarCentro
    }
    public opcion OpcionActual;
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
                if(OpcionActual == opcion.CargarTienda)
                {
                    CargarTienda();
                }
                else if (OpcionActual == opcion.CargarCentro)
                {
                    CuracionCentro.instance.CurarPokeorts();
                }

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
        MovimientoJugador movimientoJugadorScript = jugador.GetComponent<MovimientoJugador>();
        movimientoJugadorScript.enabled = false;

    }
}
