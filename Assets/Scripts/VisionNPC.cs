using UnityEngine;

public class VisionNPC : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    private Transform jugadorTransform;
    private bool persiguiendoJugador = false;

    void Update()
    {
        if (persiguiendoJugador && jugadorTransform != null)
        {
            Vector3 nuevaPosicion = Vector3.MoveTowards(
                transform.parent.position,
                jugadorTransform.position,
                velocidadMovimiento * Time.deltaTime
            );
            transform.parent.position = nuevaPosicion;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorTransform = other.transform;
            persiguiendoJugador = true;
            other.GetComponent<MovimientoJugador>().MovimientoVelocidad = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            persiguiendoJugador = false;
        }
    }
}