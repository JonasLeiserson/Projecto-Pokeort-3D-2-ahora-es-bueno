using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   [CreateAssetMenu(fileName = "New Item", menuName = "InventarioManager/Item")]
    public class Item : ScriptableObject {
    public string item;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public string tipo;
    public int valor;
}
}
