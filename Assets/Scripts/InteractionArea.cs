using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    GameObject currentInteractable;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            GameObject npc = other.gameObject;
            currentInteractable = npc;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            Debug.Log("Interacting with " + currentInteractable.name);
            DialogoTrigger dialogoTrigger = currentInteractable.GetComponent<DialogoTrigger>();
            dialogoTrigger.TriggerDialogue();
        }
    }

}
