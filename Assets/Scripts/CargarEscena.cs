using UnityEngine;
using UnityEngine.SceneManagement;

public enum Edificio
{
    Casa,
    Tienda,
    Centro,
    Mundo,
    Gimnasio,
    Laboratorio
}

public class CargarEscena : MonoBehaviour
{
    public Edificio EdificioSeleccionado;
    public string idDeSalida;

    void Start()
    {

    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (EdificioSeleccionado == Edificio.Mundo)
            {
                if (GameManager.instance != null && !string.IsNullOrEmpty(idDeSalida))
                {
                    GameManager.instance.spawnPointIDDeRetorno = idDeSalida;
                }
            }

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
                case Edificio.Mundo:
                    SceneManager.LoadScene("GameScene");
                    break;
                case Edificio.Gimnasio:
                    SceneManager.LoadScene("Gimnasio");
                    break;
                case Edificio.Laboratorio:
                    SceneManager.LoadScene("Laboratorio");
                    break;
            }
        }
    }
}