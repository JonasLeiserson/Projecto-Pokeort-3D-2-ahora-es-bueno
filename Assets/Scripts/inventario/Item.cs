using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public int valor;
    public int cantidad;
    public int ValorDeUso;
    public int id;

    public enum Tipo
    {
        curacion,
        pokeortbola,
        potenciador,
        baya,
        otro,
    }
    public Tipo tipo;

    public enum AtributoPotenciador
    {
        none,
        Ataque,
        AtaqueEspecial,
        Defensa,
        DefensaEspecial,
        Velocidad,
        Critico
    }
    public AtributoPotenciador atributoPotenciador;
}