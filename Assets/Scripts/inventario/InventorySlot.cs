using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        if (item.tipo == Item.Tipo.pokeortbola) {
            if (!CombateSalvajeManager.instance)
            {
                DialogueLine line1 = new DialogueLine();
                line1.speakerName = "Sistema";
                line1.dialogueText = "No puedes usar pokeortbolas en combates contra entrenadores.";
                CombateNPCManager.instance.dialogoCombate.dialogueLines = new List<DialogueLine> { line1 };

                PokedexUIManager.instance.EsconderEleccionpokeorts();
                PokedexUIManager.instance.ItemElejido = null;

                DialogoManager.instance.StartDialogue(CombateNPCManager.instance.dialogoCombate);
                return;
            }
            else
            {
                CombateSalvajeManager.instance.UsarPokebola(item);
                return;
            }
        }
        PokedexUIManager.instance.ShowPokeorts(item); 
        InventarioManager.instance.espaciosDePokeorts.gameObject.SetActive(true);
        PokedexUIManager.instance.UsandoItem = true;
        Inventario.instance.RemoverItem(item, 1);
        
    }
}
