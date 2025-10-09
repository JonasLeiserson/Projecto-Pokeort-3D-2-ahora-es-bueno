using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pokedex", menuName = "Pokemon/Pokedex")]
public class Pokedex : ScriptableObject
{
    [SerializeField]
    public List<PokeortInstance> pokeorts = new List<PokeortInstance>();

    public const int MAX_POKEMONS = 4;

    public void AddPokemon(PokeortInstance newPokemon)
    {
        pokeorts.Add(newPokemon);
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