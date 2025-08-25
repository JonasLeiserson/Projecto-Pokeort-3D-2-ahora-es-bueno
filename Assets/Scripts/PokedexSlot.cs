using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI pokemonNameText;
    public TextMeshProUGUI pokemonLevelText;

    private PokeortInstance currentPokeort;

    public void AddPokeort(PokeortInstance pokeort)
    {
        currentPokeort = pokeort;
        icon.sprite = pokeort.pokemonData.icon;
        pokemonNameText.text = pokeort.pokemonData.pokemonName;
        pokemonLevelText.text = "Lvl: " + pokeort.level.ToString();
    }
}