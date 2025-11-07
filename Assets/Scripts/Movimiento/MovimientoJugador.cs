using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float MovimientoVelocidad = 5f; 
    public float FuerzaSalto = 8f; 
    public float Gravedad = 20f;
    private float Stamina = 100f;
    public Animator Animador; 
    public Transform CamaraPosicion; 
    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Stamina > 30)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                MovimientoVelocidad = 10f; 
                Stamina -= 5;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MovimientoVelocidad = 5f;
        }

        transform.rotation = Quaternion.Euler(0, CamaraPosicion.eulerAngles.y, 0);

        if (controller.isGrounded)
        {
            if (Animador.GetBool("Saltando"))
                Animador.SetBool("Saltando", false);

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

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = FuerzaSalto;
                Animador.SetBool("Saltando", true);
            }
        }

        moveDirection.y -= Gravedad * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void LanzarPokebola()
    {
        Animador.SetBool("Lanzamiento", true);
    }
}
