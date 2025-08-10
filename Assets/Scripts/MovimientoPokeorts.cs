using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPokeorts : MonoBehaviour
{
    private Vector2 Velocidad;
    private float CambioDeTiempo;
    private bool EnMovimiento;
    // Start is called before the first frame update
    void Start()
    {
        EstablecerNuevaDireccion();
    }

    // Update is called once per frame
    void Update()
    {
        if(EnMovimiento)
        {
            transform.Translate(Velocidad * Time.deltaTime);
            if (Time.time > CambioDeTiempo)
            {
                Pausa();
            }
        }
        else
        {
            if (Time.time > CambioDeTiempo)
            {
                EstablecerNuevaDireccion();
            }
        }
    }
    void EstablecerNuevaDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        Vector3 direccionAleatoria = new Vector3(x, z).normalized;

        Velocidad = direccionAleatoria * Random.Range(1f, 5f);

        EnMovimiento = true;

        CambioDeTiempo = Time.time + Random.Range(4f, 6f);
    }
    void Pausa()
    {
        Velocidad = Vector3.zero;
        EnMovimiento = false;
        CambioDeTiempo = Time.time + Random.Range(4f, 6f);

    }
}
