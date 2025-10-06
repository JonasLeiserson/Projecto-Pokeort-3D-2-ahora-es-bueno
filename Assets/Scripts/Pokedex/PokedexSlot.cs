using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PokedexSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI pokemonNameText;
    public TextMeshProUGUI pokemonLevelText;
    [SerializeField] GameObject slider;
    public int index;

    PokeortInstance pokeortInstance;
    public void AddPokeort(PokeortInstance pokeort)
    {
        pokeortInstance = pokeort;

        if (pokeort.pokemonData.icon != null)
        {
            icon.sprite = pokeort.pokemonData.icon;
        }

        if (pokeort.pokemonData.pokemonName != null)
        {
            pokemonNameText.text = pokeort.pokemonData.pokemonName;
        }

        if (pokeort.level != 0)
        {
            pokemonLevelText.text = "Lvl: " + pokeort.level.ToString();
        }

        if (slider != null)
        {
            UIManager.instance.ActualizarSliderPokedex(slider, pokeort);
        }
    }
    public void Clickeado()
    {
        if (PokedexUIManager.instance.UsandoItem)
        {
            InventarioManager.instance.EsconderInventario();

            if (CombateSalvajeManager.instance)
            {
                switch (PokedexUIManager.instance.ItemElejido.tipo)
                {
                    case Item.Tipo.curacion:
                        pokeortInstance.Curar(PokedexUIManager.instance.ItemElejido.ValorDeUso, CombateSalvajeManager.instance.dialogoCombate);
                        break;
                    case Item.Tipo.potenciador:
                        pokeortInstance.Potenciar(PokedexUIManager.instance.ItemElejido.ValorDeUso, PokedexUIManager.instance.ItemElejido.atributoPotenciador);
                        break;
                    case Item.Tipo.baya:
                        break;
                    case Item.Tipo.otro:
                        break;
                }

                PokedexUIManager.instance.EsconderEleccionpokeorts();
                PokedexUIManager.instance.ItemElejido = null;
                CombateSalvajeManager combate = CombateSalvajeManager.instance;
                combate.StartCoroutine(combate.SecuenciaDeAtaqueSimple(combate.AtaqueEnemigo, combate.pokeortElegido, combate.pokeortElegidoGO));
            }
            else
            {
                switch (PokedexUIManager.instance.ItemElejido.tipo)
                {
                    case Item.Tipo.curacion:
                        pokeortInstance.Curar(PokedexUIManager.instance.ItemElejido.ValorDeUso, CombateNPCManager.instance.dialogoCombate);
                        break;
                    case Item.Tipo.potenciador:
                        pokeortInstance.Potenciar(PokedexUIManager.instance.ItemElejido.ValorDeUso, PokedexUIManager.instance.ItemElejido.atributoPotenciador);
                        break;
                    case Item.Tipo.baya:
                        break;
                    case Item.Tipo.otro:
                        break;
                }

                PokedexUIManager.instance.EsconderEleccionpokeorts();
                PokedexUIManager.instance.ItemElejido = null;
                CombateNPCManager combate = CombateNPCManager.instance;
                combate.StartCoroutine(combate.EjecutarAtaqueConDialogo(combate.AtaqueEnemigo, combate.pokeortElegido, combate.pokeortElegidoGO, UIManager.instance.sliderAmigo));
            }   
        }
        else
        {
            if (CombateSalvajeManager.instance != null)
            {
                if (index == CombateSalvajeManager.instance.indexPokeortElegido || CombateSalvajeManager.instance.pokeortAmigos[index].currentHP <= 0) 
                {
                    Debug.Log("No puedes elegir ese pokeort (vida 0 o ya esta en uso)");
                    return;
                }

                CombateSalvajeManager.instance.indexPokeortElegido = index;
                CombateSalvajeManager.instance.pokeortElegido = CombateSalvajeManager.instance.pokeortAmigos[CombateSalvajeManager.instance.indexPokeortElegido];
                CombateSalvajeManager.instance.CambiarPokeort();
                PokedexUIManager.instance.EsconderEleccionpokeorts();
            }
            else if (CombateNPCManager.instance != null)
            {
                if (index == CombateNPCManager.instance.indexPokeortElegido || CombateNPCManager.instance.pokeortAmigos[index].currentHP <= 0)
                {
                    Debug.Log("No puedes elegir ese pokeort (vida = 0 o ya esta en uso)");
                    return;
                }
                CombateNPCManager.instance.indexPokeortElegido = index;
                CombateNPCManager.instance.pokeortElegido = CombateNPCManager.instance.pokeortAmigos[CombateNPCManager.instance.indexPokeortElegido];
                CombateNPCManager.instance.CambiarPokeort();
                PokedexUIManager.instance.EsconderEleccionpokeorts();
            }

        }
    }
}