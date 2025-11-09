using UnityEngine;
using System.Collections;

public class CamaraOrbiter : MonoBehaviour
{
    public float tiempoDeVuelta = 5.0f;
    public Transform target;
    public Transform target2;

    // Distancia de la cámara al objetivo
    public float distance = 5.0f;

    // Almacena la rotación de la órbita en ángulos de Euler
    public float x = 40.0f;
    public float y = 10.0f;

    private Camera CameraJugador;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject npc = GameObject.FindGameObjectWithTag(PlayerPrefs.GetString("EncounteredPokemon"));

        if (player != null) target = player.transform;
        if (npc != null) target2 = npc.transform;

        CameraJugador = player?.GetComponentInChildren<Camera>();

        if (target == null || target2 == null)
        {
            Debug.LogError("CamaraOrbiter: Faltan referencias a target o target2.");
            return;
        }

        // ⭐ AJUSTAR el ángulo X inicial según la rotación del jugador
        if (player != null)
        {
            // Obtener la rotación Y del jugador (guardada en PlayerPrefs)
            float playerRotY = PlayerPrefs.GetFloat("RotY", 0f);
            
            // Ajustar el ángulo X de la órbita para que esté alineado con el jugador
            // -90° es la posición lateral por defecto, sumamos la rotación del jugador
            x = x + playerRotY;
            
            Debug.Log($"Rotación inicial del jugador: {playerRotY}°, ángulo X cámara: {x}°");
        }

        LateUpdate();
        DarVuelta();
    }

    void LateUpdate()
    {
        if (target == null || target2 == null) return;

        // Calcular la rotación y posición de la órbita
        Vector3 centerPoint = (target.position + target2.position) / 2.0f;
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 position = rotation * new Vector3(0.0f, 2.0f, -(distance + 4f)) + centerPoint;

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
        Debug.Log($"DarVuelta - Ángulo inicial: {initialX}°");
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
        Debug.Log($"DarVuelta - Ángulo final: {x}°");
    }
}