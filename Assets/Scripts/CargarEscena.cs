using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Edificio
{
  Casa,
  Tienda,
  Centro
}
public class CargarEscena : MonoBehaviour
{
    public Edificio EdificioSeleccionado;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (EdificioSeleccionado)
            {
                case Edificio.Casa:
                    SceneManager.LoadScene("EscenaCasa");
                    break;

                case Edificio.Tienda:
                    SceneManager.LoadScene("EscenaTienda");
                    break;

                case Edificio.Centro:
                    SceneManager.LoadScene("EscenaCentro");
                    break;
            } 
        }
    }
}
    