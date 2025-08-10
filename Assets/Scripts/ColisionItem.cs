using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionItem : MonoBehaviour
{
    public Item item;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            Debug.Log("Item Colisionado: " + item.itemName);
            Inventario.instance.Add(item);
            Destroy(gameObject);
        }
    }
}
