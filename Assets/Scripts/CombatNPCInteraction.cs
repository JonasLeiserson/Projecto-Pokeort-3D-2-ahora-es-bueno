using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatNPCInteraction : MonoBehaviour
{
    string tagNPC;

    void Start()
    {
        tagNPC = transform.parent.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerPosition = other.transform.position;
            float playerRotation = other.transform.rotation.y;

            PlayerPrefs.SetString("EncounteredPokemon", tagNPC);
            PlayerPrefs.SetFloat("PosX", playerPosition.x);
            PlayerPrefs.SetFloat("PosY", playerPosition.y);
            PlayerPrefs.SetFloat("PosZ", playerPosition.z);
            PlayerPrefs.SetFloat("RotY", playerRotation);
            SceneManager.LoadScene("CombateNPC");
        }
    }
}
