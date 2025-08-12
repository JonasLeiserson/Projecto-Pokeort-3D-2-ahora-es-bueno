using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {

    public static Inventario instance;
    public List<Item> items = new List<Item>();

    void Awake() {
        instance = this;
    }
    
    public void Add(Item item) {
        Debug.Log("anadiendo: " + item.itemName);
        Item existingItem = items.Find(x => x.itemName == item.itemName);
        
        if (existingItem != null) {
            existingItem.cantidad += item.cantidad;
        }
        else
        {
            items.Add(item);
        }
        InventarioManager.instance.UpdateUI();
    }
    
    public void Remove(Item item) {
        Debug.Log("removiendo: " + item.itemName);

        item.cantidad--;

        if (item.cantidad <= 0) {
            items.Remove(item);
        }
        InventarioManager.instance.UpdateUI();
    }

    void Start() {
        
        InventarioManager.instance.UpdateUI();
    }
}