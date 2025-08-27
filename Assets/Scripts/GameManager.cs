using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string file1 = "1";
    public string file2 = "2";
    public string file3 = "3";

    public SaveData data;

    public Pokedex pokedex;
    public Vector3 playerPosition;
    public Inventario inventory;
    public string saveFile;
    
    void Awake() 
    {
	if (instance != null && instance != this)
	{
	    Destroy(this.gameObject);
	}
	else
	{
	    instance = this;
	}
	
	DontDestroyOnLoad(this);
    }

    public void RefreshData()
    {
        data.pokedex = pokedex;
        data.playerPosition = playerPosition;
        data.inventory = inventory;
        data.saveFile = saveFile;
    }

    public void AssignData(SaveData saveData) 
    {
	    pokedex = data.pokedex;
	    playerPosition = data.playerPosition;
	    inventory = data.inventory;
	    saveFile = data.saveFile;
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
