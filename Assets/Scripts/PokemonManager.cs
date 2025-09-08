using UnityEngine;


public class PokemonManager : MonoBehaviour
{
    public PokeortData pokemonTemplate;
    public PokeortInstance currentPokemonInstance;

    void Awake()
    {
        if (pokemonTemplate != null)
        {
            currentPokemonInstance = new PokeortInstance(pokemonTemplate, 1);
        }
        else
        {
            Debug.LogError("No se ha asignado la plantilla de Pokémon al GameObject.");
        }
    }
}