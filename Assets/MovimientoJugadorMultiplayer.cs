using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated; // Importante para la clase Behavior

// 1. Heredar de la clase generada por Forge
public class MovimientoJugadorMultiplayer : JugadorForgeNetworkObjectBehavior
{
    // VARIABLES LOCALES (no necesitan sincronización automática)
    public float MovimientoVelocidad = 5f;
    public float FuerzaSalto = 8f;
    public float Gravedad = 20f;
    private float Stamina = 100f;

    public Transform CamaraPosicion; // Necesitará asignarse dinámicamente
    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Opcional: Desactivar la cámara y la escucha de la cámara de otros jugadores
        if (!IsOwner)
        {
            // Si el jugador no es local (es otro en la red), desactiva su cámara
            // Esto asume que la cámara es un componente o hijo de este objeto
            if (CamaraPosicion != null)
            {
                CamaraPosicion.gameObject.SetActive(false);
            }
        }
    }

    // 3. Método de inicio de red de Forge (es como un Start para objetos de red)
    public override void NetworkStart()
    {
        base.NetworkStart();

        // 4. Lógica de Sincronización
        // Los objetos de red que no son del dueño NO deben tener control local,
        // solo reciben actualizaciones de la posición y rotación.
        if (!IsOwner)
        {
            // Desactiva el CharacterController para que no interfiera
            // con las actualizaciones de red recibidas.
            if (controller != null)
            {
                controller.enabled = false;
            }
            return; // Detiene la ejecución de NetworkStart para clientes no propietarios
        }
    }

    void Update()
    {
        // =======================================================
        // Lógica de Input (Solo para el jugador local, IsOwner es clave)
        // =======================================================
        if (IsOwner)
        {
            // --- Lógica de Sprint y Stamina ---
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

            // --- Rotación (Controlada por el dueño) ---
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

                // --- Salto con RPC ---
                if (Input.GetButtonDown("Jump"))
                {
                    // Llama al RPC para informar a todos que hemos saltado.
                    // El servidor también lo ejecutará y lo retransmitirá.
                    // Si el RPC se llama 'Saltar', el método generado es 'RpcSaltar'.
                    RpcSaltar();

                    // Ejecuta el movimiento localmente de inmediato para que se sienta responsivo.
                    moveDirection.y = FuerzaSalto;
                }
            }

            // --- Aplicar Gravedad y Movimiento ---
            moveDirection.y -= Gravedad * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

            // =======================================================
            // 2. Sincronización de Campos (Solo el dueño actualiza el valor)
            // =======================================================
            // El servidor (Host) retransmitirá el valor del campo 'networkObject' a todos los clientes.
            networkObject.Posicion = transform.position;
            networkObject.Rotacion = transform.rotation;
        }
        else // Lógica para Clientes/Jugadores NO Propietarios
        {
            // Interpolación Suave: Mueve la posición local hacia la posición de red
            // para que el movimiento de otros jugadores se vea suave, no brusco.
            transform.position = Vector3.Lerp(transform.position, networkObject.Posicion, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkObject.Rotacion, Time.deltaTime * 15f);
        }
    }

    // =======================================================
    // 5. Implementación del RPC de Salto (Se ejecuta en todos, incluyendo el servidor)
    // =======================================================
    // Este método es obligatorio si definiste el RPC 'Saltar' en el NCW.
    public override void Saltar()
    {
        // Solo aplica la lógica de salto si el objeto es un cliente no propietario 
        // o si es el servidor (para aplicar la física a los objetos no CharacterController, si fuera el caso).
        if (!IsOwner)
        {
            // Para objetos NO propietarios, el RPC se recibe.
            // Aquí puedes ejecutar una animación de salto o un efecto visual.
            // La posición real se sincronizará a través de la interpolación de Posicion.
        }
    }
}