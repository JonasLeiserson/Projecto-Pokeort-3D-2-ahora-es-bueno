using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler 
{
    public Image icon;          
    public TextMeshProUGUI  countText;      
    Item item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventarioManager.instance.Descripcion.text = item.description;
    }
    public void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = item.icon; 
        icon.enabled = true;    

        countText.text = item.cantidad.ToString();
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;   
    }
    public void ItemUsado() {
        Debug.Log("MANGO");
        InventarioManager.instance.espaciosDePokeorts.gameObject.SetActive(true);
        Inventario.instance.RemoverItem(item, 1);
        
    }
}
