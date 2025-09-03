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
        Debug.Log("Item " + PokedexManager.instance.ItemElejido.itemName + " usado en" + pokeortInstance.pokemonData.pokemonName);

        if(PokedexManager.instance.ItemElejido.tipo == "curacion")
        {
            pokeortInstance.Curar(PokedexManager.instance.ItemElejido.ValorDeUso); 
        }
         InventarioManager.instance.EsconderEleccionpokeorts();
         PokedexManager.instance.ItemElejido = null;
    }
}