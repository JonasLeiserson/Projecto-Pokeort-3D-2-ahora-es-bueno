using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;

[System.Serializable]
public class PokeortInstance
{
    public PokeortData pokemonData;
    public List<Attack> equippedAttacks = new List<Attack>();

    // Estadísticas que varían
    public PokemonType type1;
    public PokemonType type2;
    public int level;
    public int currentHP;
    public int currentAttack;
    public int currentDefense;
    public int currentSpAttack;
    public int currentSpDefense;
    public int currentSpeed;

    public PokeortInstance(PokeortData data, int initialLevel)
    {
        this.pokemonData = data;
        this.level = initialLevel;

        type1 = data.primaryType;
        type2 = data.secondaryType;

        int hpIVs = Random.Range(0, 32);
        int attackIVs = Random.Range(0, 32);
        int defenseIVs = Random.Range(0, 32);
        int spAttackIVs = Random.Range(0, 32);
        int spDefenseIVs = Random.Range(0, 32);
        int speedIVs = Random.Range(0, 32);

        currentHP = Mathf.RoundToInt(((2 * data.baseHP + (hpIVs * (level / 100))) * level) / 100 + level + 10);
        currentAttack = Mathf.RoundToInt(((2 * data.baseAttack + (attackIVs * (level / 100))) * level) / 100 + 5);
        currentDefense = Mathf.RoundToInt(((2 * data.baseDefense + (defenseIVs * (level / 100))) * level) / 100 + 5);
        currentSpAttack = Mathf.RoundToInt(((2 * data.baseSpAttack + (spAttackIVs * (level / 100))) * level) / 100 + 5);
        currentSpDefense = Mathf.RoundToInt(((2 * data.baseSpDefense + (spDefenseIVs * (level / 100))) * level) / 100 + 5);
        currentSpeed = Mathf.RoundToInt(((2 * data.baseSpeed + (speedIVs * (level / 100))) * level) / 100 + 5);

        if (data.learnableAttacks != null && data.learnableAttacks.Count > 0)
        {
            int attacksToEquip = Mathf.Min(data.learnableAttacks.Count, 4);
            for (int i = 0; i < attacksToEquip; i++)
            {
                equippedAttacks.Add(data.learnableAttacks[i]);
            }
        }
    }

    public static class TipoEfectividad
    {
        private static readonly Dictionary<PokemonType, Dictionary<PokemonType, float>> EfectividadTipos = new Dictionary<PokemonType, Dictionary<PokemonType, float>>
        {
        { PokemonType.Agua, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 2.0f }, { PokemonType.Planta, 0.5f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Agua, 1.0f }, { PokemonType.Roca, 2.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 2.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 1.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Fuego, new Dictionary<PokemonType, float>
            {
                { PokemonType.Agua, 0.5f }, { PokemonType.Planta, 2.0f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Fuego, 1.0f }, { PokemonType.Roca, 0.5f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 0.5f }, { PokemonType.Hielo, 2.0f }, { PokemonType.Volador, 1.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Planta, new Dictionary<PokemonType, float>
            {
                { PokemonType.Agua, 2.0f }, { PokemonType.Fuego, 0.5f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Planta, 0.5f }, { PokemonType.Roca, 2.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 2.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 1.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Electrico, new Dictionary<PokemonType, float>
            {
                { PokemonType.Agua, 2.0f }, { PokemonType.Planta, 0.5f }, { PokemonType.Fuego, 1.0f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Roca, 0.5f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 0.5f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 2.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Roca, new Dictionary<PokemonType, float>
            {
                { PokemonType.Agua, 0.5f }, { PokemonType.Planta, 1.0f }, { PokemonType.Fuego, 2.0f }, { PokemonType.Electrico, 2.0f }, { PokemonType.Roca, 1.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 1.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 2.0f },
                { PokemonType.Oscuridad, 0.5f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Normal, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 1.0f }, { PokemonType.Planta, 1.0f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Agua, 1.0f }, { PokemonType.Roca, 0.5f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 1.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 1.0f },
                { PokemonType.Oscuridad, 0.5f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Tierra, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 2.0f }, { PokemonType.Planta, 0.5f }, { PokemonType.Electrico, 2.0f }, { PokemonType.Agua, 1.0f }, { PokemonType.Roca, 2.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 1.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 0.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Hielo, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 0.5f }, { PokemonType.Planta, 2.0f }, { PokemonType.Electrico, 1.0f }, { PokemonType.Agua, 0.5f }, { PokemonType.Roca, 1.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 2.0f }, { PokemonType.Hielo, 0.5f }, { PokemonType.Volador, 2.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Volador, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 1.0f }, { PokemonType.Planta, 2.0f }, { PokemonType.Electrico, 0.5f }, { PokemonType.Agua, 1.0f }, { PokemonType.Roca, 0.5f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 2.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 1.0f },
                { PokemonType.Oscuridad, 1.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Oscuridad, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 1.0f }, { PokemonType.Planta, 1.0f }, { PokemonType.Electrico, 2.0f }, { PokemonType.Agua, 1.0f }, { PokemonType.Roca, 1.0f },
                { PokemonType.Normal, 1.0f }, { PokemonType.Tierra, 1.0f }, { PokemonType.Hielo, 1.0f }, { PokemonType.Volador, 2.0f },
                { PokemonType.Oscuridad, 0.5f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Skibidi, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 100.0f }, { PokemonType.Planta, 100.0f }, { PokemonType.Electrico, 100.0f }, { PokemonType.Agua, 100.0f }, { PokemonType.Roca, 100.0f },
                { PokemonType.Normal, 100.0f }, { PokemonType.Tierra, 100.0f }, { PokemonType.Hielo, 100.0f }, { PokemonType.Volador, 100.0f },
                { PokemonType.Oscuridad, 100.0f }, { PokemonType.Skibidi, 1.0f }, { PokemonType.Especial, 1.0f }
            }
        },
        { PokemonType.Especial, new Dictionary<PokemonType, float>
            {
                { PokemonType.Fuego, 2.0f }, { PokemonType.Planta, 2.0f }, { PokemonType.Electrico, 2.0f }, { PokemonType.Agua, 2.0f }, { PokemonType.Roca, 2.0f },
                { PokemonType.Normal, 2.0f }, { PokemonType.Tierra, 2.0f }, { PokemonType.Hielo, 2.0f }, { PokemonType.Volador, 2.0f },
                { PokemonType.Oscuridad, 2.0f }, { PokemonType.Skibidi, 2.0f }, { PokemonType.Especial, 2.0f }
            }
        }
        };

        public static float ObtenerEfectividad(PokemonType tipoAtaque, PokemonType tipoDefensa1, PokemonType tipoDefensa2)
        {
            if (EfectividadTipos.ContainsKey(tipoAtaque) && EfectividadTipos[tipoAtaque].ContainsKey(tipoDefensa1) && tipoDefensa2 != PokemonType.Null)
            {
                return EfectividadTipos[tipoAtaque][tipoDefensa1] * EfectividadTipos[tipoAtaque][tipoDefensa2];
            }
            else if (EfectividadTipos.ContainsKey(tipoAtaque) && EfectividadTipos[tipoAtaque].ContainsKey(tipoDefensa1)) 
            {
              return EfectividadTipos[tipoAtaque][tipoDefensa1];
            }

        // Devolver un valor por defecto para tipos no encontrados, como Null.
        return 1.0f;
    }

    }

    public bool atacar(Attack ataque, PokeortInstance enemigo, Dialogue dialogo, DialogoManager dialogoManager)
    {
        if (ataque is DamageAttack)
        {
            DamageAttack d = (DamageAttack)ataque;

            dialogo.dialogueLines.Clear();

            bool acerto = Random.Range(0, 101) <= d.accuracy;

            if (acerto)
            {
                //calculo de danio
                bool isCritic = Random.Range(0, 101) <= d.criticChance;
                float e = TipoEfectividad.ObtenerEfectividad(d.type, enemigo.type1, enemigo.type2);
                float b = d.type == type1 ? 1.5f : 1;
                int v = Random.Range(85, 101);

                int danio;

                if (d.isPhysic)
                {
                    danio = (int)Mathf.Round(0.01f * b * e * v * (((0.2f * level + 1) * currentAttack * d.power) / (25 * enemigo.currentDefense)));
                    if (isCritic) danio *= 2;
                }
                else
                {
                    danio = (int)Mathf.Round(0.01f * b * e * v * (((0.2f * level + 1) * currentSpAttack * d.power) / (25 * enemigo.currentSpDefense)));
                    if (isCritic) danio *= 2;
                }

                enemigo.currentHP -= danio;
                if (enemigo.currentHP <= 0) enemigo.currentHP = 0;

                DialogueLine linea1 = new DialogueLine();
                linea1.dialogueText = $"{pokemonData.pokemonName} utilizo {d.attackName} e hizo {danio} de danio";

                if (isCritic) linea1.dialogueText = $"{pokemonData.pokemonName} utilizo {d.attackName} y fue critico! Hizo {danio} de danio";

                DialogueLine linea2 = new DialogueLine();
                linea2.dialogueText = $"{enemigo.pokemonData.pokemonName} tiene {enemigo.currentHP} de vida";

                dialogo.dialogueLines.Add(linea1);
                dialogo.dialogueLines.Add(linea2);

                Debug.Log($"{pokemonData.pokemonName} utilizo {d.attackName} e hizo {danio} de danio");
                Debug.Log($"{enemigo.pokemonData.pokemonName} tiene {enemigo.currentHP} de vida");

                if (enemigo.currentHP == 0)
                {
                    DialogueLine linea3 = new DialogueLine();
                    linea3.dialogueText = $"{enemigo.pokemonData.pokemonName} fue derrotado";

                    dialogo.dialogueLines.Add(linea3);
                    Debug.Log($"{enemigo.pokemonData.pokemonName} fue derrotado");

                    dialogoManager.StartDialogue(dialogo);
                    return false;
                }

                dialogoManager.StartDialogue(dialogo);
                return true;
            }
            else
            {
                DialogueLine linea1 = new DialogueLine();
                linea1.dialogueText = $"{pokemonData.pokemonName} utilizo {d.attackName} y fallo";

                DialogueLine linea2 = new DialogueLine();
                linea2.dialogueText = $"{enemigo.pokemonData.pokemonName} se mantiene con {enemigo.currentHP} de vida";

                dialogo.dialogueLines.Add(linea1);
                dialogo.dialogueLines.Add(linea2);

                Debug.Log($"{pokemonData.pokemonName} utilizo {d.attackName} y fallo");
                Debug.Log($"{enemigo.pokemonData.pokemonName} se mantiene con {enemigo.currentHP} de vida");

                dialogoManager.StartDialogue(dialogo);
                return true;
            }

        }
        else if (ataque is BuffAttack)
        {
            BuffAttack b = (BuffAttack)ataque;

            DialogueLine linea1 = new DialogueLine();
            linea1.dialogueText = $"{pokemonData.pokemonName} utilizo {b.attackName}";

            DialogueLine linea2 = new DialogueLine();
            linea2.dialogueText = $"{pokemonData.pokemonName} vio aumentado su {b.buffStat}";

            switch (b.buffStat)
            {
                case BuffAttack.buffType.Attack:
                    currentAttack = Mathf.RoundToInt(currentAttack * b.buffMultiplier);
                    break;
                case BuffAttack.buffType.Defense:
                    currentDefense = Mathf.RoundToInt(currentDefense * b.buffMultiplier);
                    break;
                case BuffAttack.buffType.Speed:
                    currentSpeed = Mathf.RoundToInt(currentSpeed * b.buffMultiplier);
                    break;
                case BuffAttack.buffType.SpecialAttack:
                    currentSpAttack = Mathf.RoundToInt(currentSpAttack * b.buffMultiplier);
                    break;
                case BuffAttack.buffType.SpecialDefense:
                    currentSpDefense = Mathf.RoundToInt(currentSpDefense * b.buffMultiplier);
                    break;
            }

            dialogo.dialogueLines.Add(linea1);
            dialogo.dialogueLines.Add(linea2);

            Debug.Log($"{pokemonData.pokemonName} utilizo {b.attackName} y vio aumentado su {b.buffStat}");
            dialogoManager.StartDialogue(dialogo);

            return true;
        }
        else if (ataque is DebuffAttack)
        {
            DebuffAttack debuff = (DebuffAttack)ataque;

            DialogueLine linea1 = new DialogueLine();
            linea1.dialogueText = $"{pokemonData.pokemonName} utilizo {debuff.attackName}";

            DialogueLine linea2 = new DialogueLine();
            linea2.dialogueText = $"{enemigo.pokemonData.pokemonName} vio reducido su {debuff.debuffStat}";

            switch (debuff.debuffStat)
            {
                case DebuffAttack.debuffType.Attack:
                    enemigo.currentAttack = Mathf.RoundToInt(enemigo.currentAttack * debuff.debuffMultiplier);
                    break;
                case DebuffAttack.debuffType.Defense:
                    enemigo.currentDefense = Mathf.RoundToInt(enemigo.currentDefense * debuff.debuffMultiplier);
                    break;
                case DebuffAttack.debuffType.Speed:
                    enemigo.currentSpeed = Mathf.RoundToInt(enemigo.currentSpeed * debuff.debuffMultiplier);
                    break;
                case DebuffAttack.debuffType.SpecialAttack:
                    enemigo.currentSpAttack = Mathf.RoundToInt(enemigo.currentSpAttack * debuff.debuffMultiplier);
                    break;
                case DebuffAttack.debuffType.SpecialDefense:
                    enemigo.currentSpDefense = Mathf.RoundToInt(enemigo.currentSpDefense * debuff.debuffMultiplier);
                    break;
            }

            dialogo.dialogueLines.Add(linea1);
            dialogo.dialogueLines.Add(linea2);

            Debug.Log($"{pokemonData.pokemonName} utilizo {debuff.attackName} y vio reducido su {debuff.debuffStat}");
            dialogoManager.StartDialogue(dialogo);

            return true;
        }

        return true;
    }
}
