using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnAtaqueScript : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnAtaque(GameObject click)
    {
        if (CombateSalvajeManager.instance != null)
        {
            CombateSalvajeManager.instance.CheckBattleState(click);
        }
    }
}
