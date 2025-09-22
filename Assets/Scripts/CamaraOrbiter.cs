using UnityEngine;
using System.Collections;

public class CamaraOrbiter : MonoBehaviour
{
    public float tiempoDeVuelta = 5.0f;
    public Transform target;

    // Distancia de la cámara al objetivo
    public float distance = 5.0f;

    // Almacena la rotación de la órbita en ángulos de Euler
    [SerializeField] private float x = 0.0f;
    [SerializeField] private float y = 0.0f;

    private Camera CameraJugador;
    void Start()
    {
        GameObject player = GameObject.Find("JugadorCamaraMan(Clone)");
        target = player.transform;
        CameraJugador = player.GetComponentInChildren<Camera>();
        DarVuelta();
    }
    void LateUpdate()
    {
        // Calcular la rotación y posición de la órbita
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        // Actualizar la transformación de la cámara
        transform.rotation = rotation;
        transform.position = position;
    }
    public void SetOrbitAngles(float newX, float newY)
    {
        x = newX;
        y = newY;
    }
    public void AdjustOrbitAngles(float deltaX, float deltaY)
    {
        x += deltaX;
        y += deltaY;
    }
    private void DarVuelta()
    {
        StartCoroutine(DarVueltaCoroutine(tiempoDeVuelta));
    }
    private IEnumerator DarVueltaCoroutine(float duration)
    {
        float initialX = x;
        float finalX = initialX + 180.0f;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            x = Mathf.Lerp(initialX, finalX, progress);

            yield return null;
        }

        x = finalX; 
        this.enabled = false;
        CameraJugador.enabled = true;
    }
}