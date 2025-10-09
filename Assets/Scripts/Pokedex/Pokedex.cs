using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pokedex", menuName = "Pokemon/Pokedex")]
public class Pokedex : ScriptableObject
{
    [SerializeField]
    public List<PokeortInstance> pokeorts = new List<PokeortInstance>();

    private const int MAX_POKEMONS = 5;

    public bool AddPokemon(PokeortInstance newPokemon)
    {
        if (pokeorts.Count < MAX_POKEMONS)
        {
            pokeorts.Add(newPokemon);
            return true;
        }
        else
        {
            return false;
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