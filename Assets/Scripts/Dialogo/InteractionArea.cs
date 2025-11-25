using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    GameObject currentInteractable;
    public GameObject interactButton;

    List<string> tags = new List<string> { "NPC", "NPCCombate", "NPCGym", "Lider" };

    void OnTriggerEnter(Collider other)
    {
        foreach (string tag in tags)
        {
            if (other.CompareTag(tag))
            {
                if (other.gameObject.GetComponent<DialogoTrigger>() != null)
                {
                    if (other.gameObject.GetComponent<DialogoTrigger>().enabled)
                    {
                        GameObject npc = other.gameObject;
                        currentInteractable = npc;
                        return;
                    }
                }
                else if (other.gameObject.GetComponent<LiderScript>() != null)
                {
                    if (other.gameObject.GetComponent<LiderScript>().enabled)
                    {
                        GameObject npc = other.gameObject;
                        currentInteractable = npc;
                        return;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (string tag in tags)
        {
            if (other.CompareTag(tag))
            {
                currentInteractable = null;
                return;
            }
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
