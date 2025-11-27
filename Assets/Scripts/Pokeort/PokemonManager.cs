using UnityEngine;


public class PokemonManager : MonoBehaviour
{
    public PokeortData pokemonTemplate;
    public PokeortInstance currentPokemonInstance;

    public void Init(int level)
    {
        if (pokemonTemplate != null)
        {
            currentPokemonInstance = new PokeortInstance(pokemonTemplate, level);
        }
        else
        {
            Debug.LogError("No se ha asignado la plantilla de Pokémon al GameObject.");
        }
    }
}