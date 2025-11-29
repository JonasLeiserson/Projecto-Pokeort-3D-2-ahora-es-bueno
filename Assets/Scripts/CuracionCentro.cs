using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuracionCentro : MonoBehaviour
{
    public Dialogue dialogo;
    public static CuracionCentro instance;
    private AudioSource AudioCurar;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
      AudioCurar = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CurarPokeorts()
    {
        Pokedex pokedex = PokedexPlayerManager.instance.pokedex;
        foreach (PokeortInstance pokeort in pokedex.pokeorts)
        {
            pokeort.currentHP = pokeort.maxHP;
        }

        DialogoManager.instance.StartDialogue(dialogo);

        AudioCurar.Play();
    }
}
