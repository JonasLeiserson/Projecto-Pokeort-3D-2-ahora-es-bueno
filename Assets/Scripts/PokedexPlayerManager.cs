using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexPlayerManager : MonoBehaviour
{
    public static PokedexPlayerManager instance;
    public Pokedex pokedex;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

