using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexUIManager : MonoBehaviour
{
    public List<PokedexSlot> slots = new List<PokedexSlot>();
    public Transform pokeortParentDer;
    public Transform pokeortParentIzq;
    public GameObject pokedexSlotPrefab;
    public Item ItemElejido;
    public bool UsandoItem = false;
    public static PokedexUIManager instance;
    public GameObject panelPokedex;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        PersistentRoot.Instance.AddToRoot(gameObject);
    }

    void Start()
    {
        EsconderEleccionpokeorts();
        UpdateUI();
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void ShowPokeorts(Item item)
    {
        ItemElejido = item;
        MostrarEleccionPokeorts();
    }


    public void UpdateUI()
    {
        foreach (Transform child in pokeortParentDer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in pokeortParentIzq)
        {
            Destroy(child.gameObject);
        }

        slots.Clear();

        int index = 0;
        foreach (PokeortInstance pokeort in PokedexPlayerManager.instance.pokedex.pokeorts)
        {
            if (pokeort == CombateNPCManager.instance.pokeortElegido || pokeort == CombateSalvajeManager.instance.pokeortElegido)
            {
                GameObject newSlot = Instantiate(pokedexSlotPrefab, pokeortParentIzq);
                PokedexSlot pokedexSlot = newSlot.GetComponent<PokedexSlot>();

                pokedexSlot.index = index;
                index++;
                slots.Add(pokedexSlot);
                pokedexSlot.AddPokeort(pokeort);
            }
            else
            {
                GameObject newSlot = Instantiate(pokedexSlotPrefab, pokeortParentDer);
                PokedexSlot pokedexSlot = newSlot.GetComponent<PokedexSlot>();

                pokedexSlot.index = index;
                index++;
                slots.Add(pokedexSlot);
                pokedexSlot.AddPokeort(pokeort);
            }
        }
    }

    public void EsconderEleccionpokeorts()
    {
        panelPokedex.gameObject.SetActive(false);

        if (CombateNPCManager.instance != null || CombateSalvajeManager.instance != null)
            UIManager.instance.combatButtons.SetActive(true);           
    }

    public void MostrarEleccionPokeorts() 
    {
        UpdateUI();
        panelPokedex.SetActive(true);
    }
}

