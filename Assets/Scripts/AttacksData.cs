using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Pokemon/Attack")]
public class Attack : ScriptableObject
{
    public string attackName;
    public PokemonType type;
    public int power;
    public int accuracy;
    public bool isPhysic;
    public int criticChance;
}

public enum PokemonType
{
    Null, Normal, Fuego, Agua, Planta, Electrico, Roca, Tierra, Hielo, Volador, Oscuridad, Skibidi, Especial
}