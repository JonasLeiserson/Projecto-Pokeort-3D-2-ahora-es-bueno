using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventarioManager : MonoBehaviour {
    public static InventarioManager instance;
    public GameObject InventarioUI; 
    public GameObject BotonesIniciales; 
    public Transform itemsParent;
    public GameObject inventorySlotPrefab;
    public TextMeshProUGUI Descripcion;
    public Transform espaciosDePokeorts;
    public List<InventorySlot> slots = new List<InventorySlot>();

    void Awake() {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void UpdateUI() {
        foreach (Transform child in itemsParent) {
            Destroy(child.gameObject);
        }
        slots.Clear();
        
        foreach (Item item in Inventario.instance.items)
        {
            if(item.cantidad <= 0)
            {
                return;
            }
            GameObject newSlot = Instantiate(inventorySlotPrefab, itemsParent);
            InventorySlot InventorySlot = newSlot.GetComponent<InventorySlot>();

            slots.Add(InventorySlot);
            InventorySlot.AddItem(item);
        }
    }

    void Start() {
        EsconderInventario();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (InventarioUI.activeSelf)
            {
                EsconderInventario();
            }
            else
            {
                MostrarInventario();
                Descripcion.text = "";
            }
        }
    }

    public void EsconderInventario()
    {
        InventarioUI.SetActive(false);
        BotonesIniciales.SetActive(true);
        
    }
    public void MostrarInventario()
    {
        InventarioUI.SetActive(true);
        BotonesIniciales.SetActive(false);
    }
    public void EsconderEleccionpokeorts()
    {
        espaciosDePokeorts.gameObject.SetActive(false);
    }
}