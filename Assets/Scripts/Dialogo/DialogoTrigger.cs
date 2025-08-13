using UnityEngine; // Necesario para MonoBehaviour, GameObject, Collider

public class DialogoTrigger : MonoBehaviour
{
    public Dialogue dialogueToTrigger;

    public void TriggerDialogue()
    {
        DialogoManager.GetInstance().StartDialogue(dialogueToTrigger);
    }
}