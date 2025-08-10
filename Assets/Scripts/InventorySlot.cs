using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;          
    public TextMeshProUGUI  countText;      
    
    Item item; 

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
}
