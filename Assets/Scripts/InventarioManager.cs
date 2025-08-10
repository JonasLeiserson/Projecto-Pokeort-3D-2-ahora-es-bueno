using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioManager : MonoBehaviour {
    public static InventarioManager instance;
    public GameObject InventarioUI; 
    public Transform itemsParent; 
    public GameObject inventorySlotPrefab;

    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake() {
        instance = this;
    }

    public void UpdateUI() {
        foreach (Transform child in itemsParent) {
            Destroy(child.gameObject);
        }
        slots.Clear();
        
        foreach (Item item in Inventario.instance.items)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, itemsParent);
            InventorySlot InventorySlot = newSlot.GetComponent<InventorySlot>();

            slots.Add(InventorySlot);
            InventorySlot.AddItem(item);
        }
    }
}