using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {

    public static Inventario instance;
    public List<Item> items = new List<Item>();

    void Awake() {
        instance = this;
    }
    
    public void AñadirItem(Item item, int Cantidad) {
        Debug.Log("anadiendo: " + item.itemName);
        Item existingItem = items.Find(x => x.itemName == item.itemName);
        
        if (existingItem != null) {
            existingItem.cantidad = item.cantidad + Cantidad;
        }
        else
        {
            items.Add(item);
            existingItem.cantidad += item.cantidad;
        }
        InventarioManager.instance.UpdateUI();
    }
    
    public void RemoverItem(Item item, int Cantidad) {
        Debug.Log("removiendo: " + item.itemName);

        item.cantidad = item.cantidad - Cantidad;

        if (item.cantidad <= 0) {
            items.Remove(item);
        }
        InventarioManager.instance.UpdateUI();
    }

    void Start() {
        
        InventarioManager.instance.UpdateUI();
    }
}