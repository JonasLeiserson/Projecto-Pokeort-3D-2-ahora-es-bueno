using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexManager : MonoBehaviour
{
    public Pokedex pokedex;
    public List<PokedexSlot> slots = new List<PokedexSlot>(); 
    public Transform pokeortParent;
    public GameObject pokedexSlotPrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
	    GameObject pokeortParentOb = GameObject.Find("PokeortsDerecho");
	    pokeortParent = pokeortParentOb.transform;
	    InventarioManager.instance.EsconderEleccionpokeorts();
        UpdateUI();
    }


    // Update is called once per frame
    void Update()
    {
        
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
            PokedexSlot pokedexSlot = newSlot.GetComponent<PokedexSlot>();

            slots.Add(pokedexSlot);
            pokedexSlot.AddPokeort(pokeort);
        }
    }
}
