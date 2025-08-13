using UnityEngine;

public class CameraTraker : MonoBehaviour
{
    private Vector2 Angulo = new Vector2(90 * Mathf.Deg2Rad, 0);
    public Transform Seguir;
    public float Distancia;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float Horizontal = Input.GetAxis("Mouse X");

        if (Horizontal != 0)
        {
            Angulo.x += Horizontal * Mathf.Deg2Rad;
        }

        float Vertical = Input.GetAxis("Mouse Y");

        if (Vertical != 0)
        {
            Angulo.y += Vertical * Mathf.Deg2Rad;
            Angulo.y = Mathf.Clamp(Angulo.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);
        }
    }
    void LateUpdate()
    {
        Vector3 orbita = new Vector3(
            Mathf.Cos(Angulo.x) * Mathf.Cos(Angulo.y),
            -Mathf.Sin(Angulo.y),
            -Mathf.Sin(Angulo.x) * Mathf.Cos(Angulo.y)
        );

        transform.position = Seguir.position + orbita * Distancia;
        transform.rotation = Quaternion.LookRotation(Seguir.position - transform.position);
    }
}