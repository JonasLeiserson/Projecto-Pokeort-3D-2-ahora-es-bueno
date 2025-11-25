using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    GameObject currentInteractable;
    public GameObject interactButton;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            GameObject npc = other.gameObject;
            currentInteractable = npc;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            currentInteractable = null;
        }
    }

    private void Update()
    {
        if (currentInteractable != null && !DialogoManager.instance.talking)
        {
            interactButton.SetActive(true);
        }
        else
        {
            interactButton.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            Debug.Log("Interacting with " + currentInteractable.name);
            DialogoTrigger dialogoTrigger = currentInteractable.GetComponent<DialogoTrigger>();
            dialogoTrigger.TriggerDialogue();
            interactButton.SetActive(false);
        }
    }

}
