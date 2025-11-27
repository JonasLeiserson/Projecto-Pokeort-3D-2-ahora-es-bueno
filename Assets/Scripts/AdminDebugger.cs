using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminDebugger : MonoBehaviour
{
    public PokedexPlayerManager pokedexPlayerManager;
    public GameObject[] pokeorts;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("EscenaCasa");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("EscenaTienda");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("EscenaCentro");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Gimnasio");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Laboratorio");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = new Vector3(177.0843f, 3f, 18.66978f);

            if (cc != null) cc.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject pokeort = Instantiate(pokeorts[0]);
            pokeort.GetComponent<PokemonManager>().Init(5);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject pokeort = Instantiate(pokeorts[1]);
            pokeort.GetComponent<PokemonManager>().Init(3);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject pokeort = Instantiate(pokeorts[2]);
            pokeort.GetComponent<PokemonManager>().Init(3);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject pokeort = Instantiate(pokeorts[3]);
            pokeort.GetComponent<PokemonManager>().Init(3);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject pokeort = Instantiate(pokeorts[4]);
            pokeort.GetComponent<PokemonManager>().Init(4);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject pokeort = Instantiate(pokeorts[5]);
            pokeort.GetComponent<PokemonManager>().Init(5);
            pokedexPlayerManager.pokedex.AddPokemon(pokeort.GetComponent<PokemonManager>().currentPokemonInstance);
        }
    }
}
