using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlataManager : MonoBehaviour
{
    public static PlataManager instance;
    public int PlataJugador = 300; 
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EscenaTienda")
        {
            UIManager.instance.ActualizarTextPlata(PlataJugador);
        }
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
