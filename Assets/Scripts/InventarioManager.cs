using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour {
    public static InventarioManager instance;
    public GameObject InventarioUI; 
    public Transform itemsParent; 
    public InventorySlot[] slots; 

    void Awake() {
        instance = this;
    }

    void Start() {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void UpdateUI() {
        for (int i = 0; i < slots.Length; i++) {
            if (i < Inventario.instance.items.Count) {
                slots[i].AddItem(Inventario.instance.items[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
    }
}