using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon/Pokemon")]
public class PokeortData : ScriptableObject
{
    public GameObject PokeortPrefab;
    public string pokemonName;
    public PokemonType primaryType;
    public PokemonType secondaryType;
    public Sprite icon;

    public int baseHP;
    public int baseAttack;
    public int baseDefense;
    public int baseSpAttack;
    public int baseSpDefense;
    public int baseSpeed;

    public List<Attack> learnableAttacks;
}