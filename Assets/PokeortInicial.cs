using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokeortInicial : MonoBehaviour
{
    GameObject currentInteractable;
    public GameObject interactButton;
    public GameObject canva;
    public TextMeshProUGUI pokeortName;
    public Image pokeortImage;
    public GameObject aceptar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentInteractable != null && !canva.activeSelf && !DialogoManager.instance.talking) 
        {
            interactButton.SetActive(true);
        }
        else
        {
            interactButton.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            PokeortData pokeortData = currentInteractable.GetComponent<PokemonManager>().pokemonTemplate;
            pokeortName.text = pokeortData.pokemonName;
            pokeortImage.sprite = pokeortData.icon;

            canva.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            currentInteractable.GetComponent<PokemonManager>().Init(5);
            PokeortInstance instance = currentInteractable.GetComponent<PokemonManager>().currentPokemonInstance;

            aceptar.GetComponent<BotonAceptarInicial>().instance = instance;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            currentInteractable = gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            currentInteractable = null;
        }
    }
}
