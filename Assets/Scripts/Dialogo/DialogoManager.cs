using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;

public class DialogoManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText; 
    public TextMeshProUGUI dialogueText;
    public Image speakerPortraitImage;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    public bool talking;

    private static DialogoManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        dialoguePanel.SetActive(false);
    }

    public static DialogoManager GetInstance()
    {
        return instance;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        talking = true;
        dialoguePanel.SetActive(true);
        DisplayDialogueLine();
    }

    // Método principal para mostrar una línea de diálogo
    void DisplayDialogueLine()
    {
        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.dialogueLines[currentLineIndex];

        speakerNameText.text = line.speakerName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.dialogueText));

        if (speakerPortraitImage != null)
        {
            speakerPortraitImage.sprite = line.speakerPortrait;
            speakerPortraitImage.gameObject.SetActive(line.speakerPortrait != null);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
    }
    public void NextLine()
    {
            currentLineIndex++;
            DisplayDialogueLine();
    }

    void EndDialogue()
    {
        talking = false;
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == currentDialogue.dialogueLines[currentLineIndex].dialogueText)
            {
                NextLine();
            }
        }
    }
}