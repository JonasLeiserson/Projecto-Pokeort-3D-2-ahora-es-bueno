using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI pokemonNameText;
    public TextMeshProUGUI pokemonLevelText;

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
    }
    public void Clickeado()
    {
        if(PokedexUIManager.instance.UsandoItem == true)
        {
            Debug.Log("Item " + PokedexUIManager.instance.ItemElejido.itemName + " usado en" + pokeortInstance.pokemonData.pokemonName);

            switch (PokedexUIManager.instance.ItemElejido.tipo)
            {
                case Item.Tipo.curacion:
                    pokeortInstance.Curar(PokedexUIManager.instance.ItemElejido.ValorDeUso);
                    break;
                case Item.Tipo.pokeortbola:
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
        }
        else
        {
            CombateSalvajeManager.instance.indexPokeortElegido = 0;
            CombateSalvajeManager.instance.pokeortElegido =  CombateSalvajeManager.instance.pokeortAmigos[CombateSalvajeManager.instance.indexPokeortElegido];
        }
    }
}