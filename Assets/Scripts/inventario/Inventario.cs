using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {

    public static Inventario instance;
    public List<Item> items = new List<Item>();

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    
    public void AñadirItem(Item item, int Cantidad) {
        Debug.Log("Anadiendo: " + item.itemName);
        
        Item existingItem = items.Find(x => x.itemName == item.itemName);
        
        if (existingItem != null) {
            existingItem.cantidad += Cantidad;
        }
        else {
            items.Add(item);
            item.cantidad = Cantidad;
        }

        InventarioManager.instance.UpdateUI();
    }
    
    public void RemoverItem(Item item, int Cantidad) {
        Debug.Log("Removiendo: " + item.itemName);
        
        Item existingItem = items.Find(x => x.itemName == item.itemName);

        if (existingItem != null) {
            existingItem.cantidad -= Cantidad;

            if (existingItem.cantidad <= 0) {
                items.Remove(existingItem);
            }
        }
        
        InventarioManager.instance.UpdateUI();
    }

    void Start() {
        InventarioManager.instance.UpdateUI();
    }
}