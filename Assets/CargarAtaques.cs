using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CargarAtaques : MonoBehaviour
{
    public static CargarAtaques instance;

    UIManager uiManager = UIManager.instance;
    CombateSalvajeManager combateSalvajeManager = CombateSalvajeManager.instance;

    public Button botonAtaque;
    public GameObject botonesIniciales;
    public GameObject botonesAtaque;
    public GameObject combatButtons;

    void Awake()
    {
        if (instance == null && instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CargarAtaquesUI() => uiManager.CargarAtaques(combateSalvajeManager.pokeortElegido.equippedAttacks, botonAtaque, botonesAtaque, botonesIniciales);

    public void EsconderAtaques() => uiManager.EsconderAtaques(botonesIniciales, botonesAtaque);
}
