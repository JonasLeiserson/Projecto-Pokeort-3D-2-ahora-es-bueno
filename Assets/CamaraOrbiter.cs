using UnityEngine;

public class CameraOrbiter : MonoBehaviour
{
    // El objetivo alrededor del cual orbitará la cámara
    public Transform target;
    
    // Distancia de la cámara al objetivo
    public float distance = 5.0f;
    
    // Almacena la rotación de la órbita en ángulos de Euler
    [SerializeField] private float x = 0.0f;
    [SerializeField] private float y = 0.0f;

    void LateUpdate()
    {
        // Solo orbita si se ha asignado un objetivo
        if (target == null) return;

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
    }
}