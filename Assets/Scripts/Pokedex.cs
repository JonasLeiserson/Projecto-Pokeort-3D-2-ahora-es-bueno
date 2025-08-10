using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pokedex", menuName = "Pokemon/Pokedex")]
public class Pokedex : ScriptableObject
{
    [SerializeField]
    public List<PokeortInstance> pokeorts = new List<PokeortInstance>();

    private const int MAX_POKEMONS = 3;

    public void AddPokemon(PokeortInstance newPokemon)
    {
        if (pokeorts.Count < MAX_POKEMONS)
        {
            pokeorts.Add(newPokemon);
            Debug.Log(newPokemon.pokemonData.pokemonName + " ha sido añadido al inventario.");
        }
        else
        {
            Debug.Log("El inventario está lleno. No se puede añadir a " + newPokemon.pokemonData.pokemonName);
        }
    }

    public PokeortInstance GetPokemon(int index)
    {
        if (index >= 0 && index < pokeorts.Count)
        {
            return pokeorts[index];
        }
        return null;
    }
}