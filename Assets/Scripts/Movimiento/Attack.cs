using UnityEngine;

public class Attack : ScriptableObject
{
    public string attackName;
    public PokemonType type;
}

[CreateAssetMenu(menuName = "Pokemon/Attack/Damage")]
public class DamageAttack : Attack
{
    public int power;
    public int accuracy;
    public bool isPhysic;
    public int criticChance;
}

[CreateAssetMenu(menuName = "Pokemon/Attack/Buff")]
public class BuffAttack : Attack
{
    public float buffMultiplier;
    public enum buffType
    {
        Attack,
        Defense,
        Speed,
        SpecialAttack,
        SpecialDefense
    }
    public buffType buffStat;
}

[CreateAssetMenu(menuName = "Pokemon/Attack/Debuff")]
public class DebuffAttack : Attack
{
    public float debuffMultiplier;
    public enum debuffType
    {
        Attack,
        Defense,
        Speed,
        SpecialAttack,
        SpecialDefense
    }
    public debuffType debuffStat;
}


public enum PokemonType
{
    Null, Normal, Fuego, Agua, Planta, Electrico, Roca, Tierra, Hielo, Volador, Oscuridad, Skibidi, Especial
}