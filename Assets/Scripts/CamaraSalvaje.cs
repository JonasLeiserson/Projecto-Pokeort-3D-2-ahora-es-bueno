using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSalvaje : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(-5, 5, 0);
    public float rotationAngle = 165;

    void Start()
    {
        GameObject player = GameObject.Find("JugadorCamaraMan(Clone)");
        if (player != null)
        {
            target = player.transform;

            
            transform.position = target.position + offset;

            transform.rotation = Quaternion.Euler(25, rotationAngle, 0);
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador");
        }
    }
}
