using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexManagerNPC : MonoBehaviour
{
    public Pokedex pokedex;
    private Rigidbody rb;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float velocidad = rb.velocity.magnitude;
        Animator animator = GetComponentInChildren<Animator>();

        // 🔹 SOLUCIÓN: Solo rotar en el eje Y y no interferir con el Rigidbody
        if (velocidad > 0.1f)
        {
            Vector3 direccionMovimiento = rb.velocity.normalized;
            direccionMovimiento.y = 0; // Solo movimiento horizontal

            if (direccionMovimiento != Vector3.zero)
            {
                // Calcular solo el ángulo Y para modelos que miran hacia -X
                float anguloY = Mathf.Atan2(direccionMovimiento.x, direccionMovimiento.z) * Mathf.Rad2Deg + 90f;

                // Solo modificar la rotación Y, manteniendo X y Z del Rigidbody
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, anguloY, Time.deltaTime * 5f);
                transform.eulerAngles = eulerAngles;
            }
        }
    }
}
