using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncuentroPokemon : MonoBehaviour
{
    string escenaCombate = "CombateSalvaje";
    string tagPokeort;
    private bool hasLoadedScene = false;

    // Start is called before the first frame update

    void Start()
    {
        tagPokeort = gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Vector3 playerPosition = other.transform.position;
            float playerRotation = other.transform.rotation.y;

            PlayerPrefs.SetString("EncounteredPokemon", tagPokeort);
            PlayerPrefs.SetFloat("PosX", playerPosition.x);
            PlayerPrefs.SetFloat("PosY", playerPosition.y);
            PlayerPrefs.SetFloat("PosZ", playerPosition.z);
            PlayerPrefs.SetFloat("RotY", playerRotation);

            if (!hasLoadedScene)
            {
                SceneManager.LoadScene(escenaCombate);
            }
        }
    }
}
