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
            if (animator != null)
            {
                animator.SetFloat("Velocidad", velocidad);
            }

            if (animator.GetFloat("Velocidad") == velocidad)
            {
                transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

                // 🔹 NUEVO: Rotar hacia la dirección de movimiento (ajustado para modelos que miran hacia -X)
                if (direccion != Vector3.zero)
                {
                    // Como los modelos miran hacia -X, necesitamos ajustar la rotación
                    // Convertimos la dirección de movimiento a la orientación correcta del modelo
                    Vector3 direccionAjustada = new Vector3(direccion.z, direccion.y, -direccion.x);
                    Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionAjustada);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
                }

                if (Time.time > cambioDeTiempo)
                {
                    if (animator != null)
                    {
                        animator.SetFloat("Velocidad", velocidad);
                    }
                    Pausa();
                }
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetFloat("Velocidad", velocidad);
            }
            if (animator.GetFloat("Velocidad") == velocidad)
            {
                if (Time.time > cambioDeTiempo)
                {
                    EstablecerNuevaDireccion();
                }
            }
        }
    }

    void EstablecerNuevaDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        direccion = new Vector3(x, 0f, z).normalized;
        velocidad = Random.Range(1f, 5f);

        // 🔹 Enviamos la velocidad al Animator (si existe)

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
