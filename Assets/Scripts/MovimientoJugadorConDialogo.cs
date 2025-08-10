using UnityEngine;

public class MovimientoJugadorConDialogo : MonoBehaviour
{
    public float MovimientoVelocidad = 5f;
    public float FuerzaSalto = 8f;
    public float Gravedad = 20f;

    public Transform CamaraPosicion;
    private CharacterController controller;
    private Vector3 moveDirection;


    DialogoManager dialogoManager;
    bool talking = false;

    void Start()
    {
        dialogoManager = DialogoManager.GetInstance();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!talking)
        {
            transform.rotation = Quaternion.Euler(0, CamaraPosicion.eulerAngles.y, 0);

            if (controller.isGrounded)
            {
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
                }
            }

            moveDirection.y -= Gravedad * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);
        }
    }
            
}
