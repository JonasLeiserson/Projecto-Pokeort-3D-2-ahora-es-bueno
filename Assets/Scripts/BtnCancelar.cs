using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCancelar : MonoBehaviour
{
    public void EsconderAtaques()
    {
        UIManager.instance.EsconderAtaques();
    }

    public void EsconderPokedex()
    {
        UIManager.instance.EsconderCambioPokeort();
        PokedexUIManager.instance.UsandoItem = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
