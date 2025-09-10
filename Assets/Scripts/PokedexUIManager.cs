using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexUIManager : MonoBehaviour
{
    public Pokedex pokedex;
    public List<PokedexSlot> slots = new List<PokedexSlot>();
    public Transform pokeortParent;
    public GameObject pokedexSlotPrefab;
    public Item ItemElejido;
<<<<<<< Updated upstream:Assets/Scripts/PokedexManager.cs
    public bool UsandoItem;
    public static PokedexManager instance;
=======
    public static PokedexUIManager instance;
>>>>>>> Stashed changes:Assets/Scripts/PokedexUIManager.cs
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        GameObject pokeortParentOb = GameObject.Find("PokeortsDerecho");
        pokeortParent = pokeortParentOb.transform;
        EsconderEleccionpokeorts();
        UpdateUI();
    }


    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream:Assets/Scripts/PokedexManager.cs
    }
        public void ShowPokeorts(Item item)
=======

    }

    public void ShowPokeorts(Item item)
>>>>>>> Stashed changes:Assets/Scripts/PokedexUIManager.cs
    {
        ItemElejido = item;
        InventarioManager.instance.espaciosDePokeorts.gameObject.SetActive(true);
    }


    public void UpdateUI()
    {
        foreach (Transform child in pokeortParent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        foreach (PokeortInstance pokeort in pokedex.pokeorts)
        {
            GameObject newSlot = Instantiate(pokedexSlotPrefab, pokeortParent);
            newSlot.transform.localScale = new Vector3(0.5f, 0.3f, 0.3f);
            PokedexSlot pokedexSlot = newSlot.GetComponent<PokedexSlot>();

            slots.Add(pokedexSlot);
            pokedexSlot.AddPokeort(pokeort);
        }
    }

    public void EsconderEleccionpokeorts()
    {
        pokeortParent.gameObject.SetActive(false);
    }
}

