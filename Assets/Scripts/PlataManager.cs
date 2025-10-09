using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataManager : MonoBehaviour
{
    public static PlataManager instance;
    public int PlataJugador = 0; 
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void AgregarPlata(int Plata)
    {
        PlataJugador += Plata;
        UIManager.instance.ActualizarTextPlata(PlataJugador);
    }
    public void QuitarDinero(int Plata)
    {
        PlataJugador = Mathf.Max(0, PlataJugador - Plata);
        UIManager.instance.ActualizarTextPlata(PlataJugador);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlataJugador += 100;
            UIManager.instance.ActualizarTextPlata(PlataJugador);
        }

    }
}
