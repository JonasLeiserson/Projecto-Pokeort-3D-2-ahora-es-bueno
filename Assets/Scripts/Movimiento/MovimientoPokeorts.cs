using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPokeorts : MonoBehaviour
{
    public float velocidad = 0f; // Valor público para el Animator
    private Vector3 direccion;
    private float cambioDeTiempo;
    private bool enMovimiento;

    private Animator animator;

    void Start()
    {
        // 🔹 Busca el Animator en los hijos del GameObject
        animator = GetComponentInChildren<Animator>();
        EstablecerNuevaDireccion();
    }

    void Update()
    {
        if (enMovimiento)
        {
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

            if (Time.time > cambioDeTiempo)
            {
                Pausa();
            }
        }
        else
        {
            if (Time.time > cambioDeTiempo)
            {
                EstablecerNuevaDireccion();
            }
        }

        // 🔹 Enviamos la velocidad al Animator (si existe)
        if (animator != null)
        {
            animator.SetFloat("Velocidad", velocidad);
        }
    }

    void EstablecerNuevaDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        direccion = new Vector3(x, 0f, z).normalized;
        velocidad = Random.Range(1f, 5f);

        enMovimiento = true;
        cambioDeTiempo = Time.time + Random.Range(4f, 6f);
    }

    void Pausa()
    {
        velocidad = 0f;
        enMovimiento = false;
        cambioDeTiempo = Time.time + Random.Range(4f, 6f);
    }
}
