using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexManagerNPC : MonoBehaviour
{
    public Pokedex pokedex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float velocidad = GetComponent<Rigidbody>().velocity.magnitude;
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetFloat("Velocidad", velocidad); 
    }
}
