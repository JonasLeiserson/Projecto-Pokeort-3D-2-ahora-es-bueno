using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncuentroPokemon : MonoBehaviour
{
    string escenaCombate = "Combate";
    string tagPokeort;
    private bool hasLoadedScene = false;

    void Start()
    {
        // Guardar el tag del objeto que representa al pokeort
        tagPokeort = gameObject.tag;

        if (string.IsNullOrEmpty(tagPokeort))
        {
            Debug.LogError($"⚠️ El objeto {gameObject.name} no tiene un tag asignado. Asigna un tag válido en el inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasLoadedScene) return; // evitar doble entrada

        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(tagPokeort))
            {
                // Guardar info del Pokeort encontrado
                PlayerPrefs.SetString("EncounteredPokemon", tagPokeort);

                // Guardar posición y rotación del jugador
                Vector3 playerPosition = other.transform.position;
                float playerRotation = other.transform.rotation.eulerAngles.y;

                PlayerPrefs.SetFloat("PosX", playerPosition.x);
                PlayerPrefs.SetFloat("PosY", playerPosition.y);
                PlayerPrefs.SetFloat("PosZ", playerPosition.z);
                PlayerPrefs.SetFloat("RotY", playerRotation);

                PlayerPrefs.Save();

                hasLoadedScene = true;
                SceneManager.LoadScene(escenaCombate);
            }
            else
            {
                Debug.LogError($"❌ El encuentro falló porque {gameObject.name} no tiene un tag válido.");
            }
        }
    }
}
