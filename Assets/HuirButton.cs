using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuirButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Huir(Dialogue dialogo)
    {
        if (!CombateSalvajeManager.instance)
        {
            DialogueLine line1 = new DialogueLine();
            line1.dialogueText = "No puedes huir del combate en una batalla contra otro entrenador";

            dialogo.dialogueLines = new List<DialogueLine> { line1 };

            DialogoManager.instance.StartDialogue(dialogo);
        }
        else
        {
            int r = Random.Range(0, 100);

            if (r < 50)
            {
                DialogueLine line1 = new DialogueLine();
                line1.dialogueText = "Has huido con exito";
                dialogo.dialogueLines = new List<DialogueLine> { line1 };
                DialogoManager.instance.StartDialogue(dialogo);
                CombateSalvajeManager.instance.HuirCombate();
            }
            else
            {
                DialogueLine line1 = new DialogueLine();
                line1.dialogueText = "No has podido huir del combate";
                dialogo.dialogueLines = new List<DialogueLine> { line1 };
                DialogoManager.instance.StartDialogue(dialogo);

                StartCoroutine(CombateSalvajeManager.instance.SecuenciaDeAtaqueSimple(CombateSalvajeManager.instance.AtaqueEnemigo, CombateSalvajeManager.instance.pokeortElegido, CombateSalvajeManager.instance.pokeortElegidoGO));
            }
        }
    }
}
