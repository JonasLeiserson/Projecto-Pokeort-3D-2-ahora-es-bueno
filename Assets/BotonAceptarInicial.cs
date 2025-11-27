using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BotonAceptarInicial : MonoBehaviour
{
    public PokeortInstance instance;
    public Dialogue dialogo;
    public GameObject canva;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aceptar()
    {
        UnityEngine.Debug.Log(instance);

        PokedexPlayerManager.instance.pokedex.pokeorts.Add(instance);
        canva.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject[] bolas = GameObject.FindGameObjectsWithTag("Bola");

        foreach (GameObject bola in bolas)
        {
            Destroy(bola);
        }

        DialogueLine line1 = new DialogueLine();
        line1.speakerName = "Darin";
        line1.dialogueText = "¡Excelente elección! Estoy seguro de que " + instance.pokemonData.pokemonName + " y tú serán un gran equipo.";
        dialogo.dialogueLines = new List<DialogueLine> { line1 };

        DialogoManager.instance.StartDialogue(dialogo);

    }


    public void Rechazar()
    {
        canva.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
