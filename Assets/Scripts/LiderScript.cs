using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiderScript : MonoBehaviour
{
    string tagNPC;
    public bool hasTalked = false;
    public bool fueDerrotado = false;

    public Dialogue dialogueToTrigger1;
    public Dialogue dialogueToTrigger2;

    IEnumerator WaitDialogue()
    {
        yield return new WaitUntil(() => !DialogoManager.instance.talking);

        if (!fueDerrotado)
        {
            hasTalked = true;
        }
    }

    public void TriggerDialogue(Dialogue dialogo)
    {
        DialogoManager.GetInstance().StartDialogue(dialogo);

        StartCoroutine(WaitDialogue());
    }

    void Start()
    {
        tagNPC = gameObject.tag;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && hasTalked && !fueDerrotado)
        {
                Vector3 playerPosition = other.transform.position;
                Vector3 playerRotation = other.transform.rotation.eulerAngles;

                // Guardar el NOMBRE del prefab del padre, no el tag
                string npcPrefabName = name.Replace("(Clone)", "").Trim();

                PlayerPrefs.SetString("EncounteredPokemon", npcPrefabName);
                PlayerPrefs.SetFloat("PosX", playerPosition.x);
                PlayerPrefs.SetFloat("PosY", playerPosition.y);
                PlayerPrefs.SetFloat("PosZ", playerPosition.z);
                PlayerPrefs.SetFloat("RotY", playerRotation.y);
                Debug.Log(playerRotation);
                Debug.Log($"Tag guardado: {tagNPC}, Nombre del padre: {name}");

                GameManager.instance.RefreshData();
                SceneManager.LoadScene("CombateGimnasio");       
        }
    }
}