using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GymCombatScript : MonoBehaviour
{
    string tagNPC;

    void Start()
    {
        tagNPC = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
            Debug.Log($"Tag guardado: {tagNPC}, Nombre del padre: {name}");

            GameManager.instance.RefreshData();
            SceneManager.LoadScene("CombateGimnasio");
        }
    }
}