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
        items.Add(item);
        InventarioManager.instance.UpdateUI();
    }
    
    public void Remove(Item item) {
        items.Remove(item);
        InventarioManager.instance.UpdateUI();
    }
}