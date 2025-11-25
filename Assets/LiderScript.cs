using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiderScript : MonoBehaviour
{
    string tagNPC;
    bool hasTalked = false;

    public Dialogue dialogueToTrigger;

    IEnumerator WaitDialogue()
    {
        yield return new WaitUntil(() => !DialogoManager.GetInstance().talking);
    }

    public void TriggerDialogue()
    {
        DialogoManager.GetInstance().StartDialogue(dialogueToTrigger);

        StartCoroutine(WaitDialogue());
        hasTalked = true;
    }

    void Start()
    {
        tagNPC = transform.parent.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasTalked)
        {
            Vector3 playerPosition = other.transform.position;
            Vector3 playerRotation = other.transform.rotation.eulerAngles;

            // Guardar el NOMBRE del prefab del padre, no el tag
            string npcPrefabName = transform.parent.name.Replace("(Clone)", "").Trim();

            PlayerPrefs.SetString("EncounteredPokemon", npcPrefabName);
            PlayerPrefs.SetFloat("PosX", playerPosition.x);
            PlayerPrefs.SetFloat("PosY", playerPosition.y);
            PlayerPrefs.SetFloat("PosZ", playerPosition.z);
            PlayerPrefs.SetFloat("RotY", playerRotation.y);
            Debug.Log(playerRotation);
            Debug.Log($"Tag guardado: {tagNPC}, Nombre del padre: {transform.parent.name}");

            GameManager.instance.RefreshData();
            SceneManager.LoadScene("Combate");
        }
    }

}