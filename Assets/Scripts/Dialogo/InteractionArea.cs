using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionArea : MonoBehaviour
{
    GameObject currentInteractable;
    public GameObject interactButton;

    List<string> tags = new List<string> { "NPC", "NPCCombate", "NPCGym", "Lider", "NPCCombate2", "NPCGym2" };

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            foreach (Transform t in canvas.transform)
            {
                if (t.name == "Interact")
                {
                    interactButton = t.gameObject;
                }
            }
        }
    }

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
                    Debug.Log("hola");
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
        if (interactButton != null)
        {
            if (currentInteractable != null && !DialogoManager.instance.talking && !PauseMenuManager.instance.pauseMenuUI.activeSelf)
                if (currentInteractable.GetComponent<LiderScript>() != null)
                {
                    if (!currentInteractable.GetComponent<LiderScript>().hasTalked)
                    {
                        interactButton.SetActive(true);
                    }
                    else
                    {
                        interactButton.SetActive(false);
                    }
                } 
                else
                {
                    interactButton.SetActive(true);
                }
            else
                    interactButton.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            Debug.Log("Interacting with " + currentInteractable.name);
            if (currentInteractable.GetComponent<DialogoTrigger>() != null)
            {
                DialogoTrigger dialogoTrigger = currentInteractable.GetComponent<DialogoTrigger>();
                dialogoTrigger.TriggerDialogue();
            }
            else if (currentInteractable.GetComponent<LiderScript>() != null)
            {
                LiderScript liderScript = currentInteractable.GetComponent<LiderScript>();
                liderScript.TriggerDialogue();
            }

            interactButton.SetActive(false);
        }
    }

}
