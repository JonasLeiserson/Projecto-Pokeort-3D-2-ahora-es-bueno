using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float MovimientoVelocidad = 5f; 
    public float FuerzaSalto = 8f; 
    public float Gravedad = 20f;
    public Animator Animador; 
    public Transform CamaraPosicion; 
    private CharacterController controller;
    private Vector3 moveDirection;
    float velocidad;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float distancia = 10f;

        Vector3 direccion = -transform.right;

        // Hacer el raycast
        if (Physics.Raycast(transform.position, direccion, out RaycastHit hitInfo, distancia))
        {
            // Si pega, lo dibuja en rojo
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        }
        else
        {
            // Si no pega, lo dibuja en verde hasta la distancia máxima
            Debug.DrawLine(transform.position, transform.position + direccion * distancia, Color.green);
        }

        transform.rotation = Quaternion.Euler(0, CamaraPosicion.eulerAngles.y + 90, 0);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 forward = CamaraPosicion.forward;
            Vector3 right = CamaraPosicion.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * vertical + right * horizontal;
            moveDirection = desiredMoveDirection * MovimientoVelocidad;

        moveDirection.y -= Gravedad * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        velocidad = controller.velocity.magnitude;
        Animador.SetFloat("Velocidad", velocidad);
    }

    public void LanzarPokebola()
    {
        Animador.SetBool("Lanzamiento", true);
    }
}
