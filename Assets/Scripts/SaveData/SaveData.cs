using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public List<PokeortInstance> pokeorts;
    public Vector3 playerPosition;
    public List<Item> inventory;
    public string saveFile;
}


public static class SaveSystem {
    static string GetSavePath(string saveFile) {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string saveFolder = Path.Combine(folder, "PokeORT 3D");
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        return Path.Combine(saveFolder, saveFile);
    }

    public static void SaveGame(SaveData data) {
        string json = JsonUtility.ToJson(data); 
        File.WriteAllText(GetSavePath(data.saveFile), json);
    }

    public static SaveData InitSaveFileIfNeeded(string file) {
        string path = GetSavePath(file);
        if (!File.Exists(path)) {
            GameManager.instance.pokedex.pokeorts.Clear();
            Inventario.instance.items.Clear();
            SaveData initialData = new SaveData 
            { 
                pokeorts = GameManager.instance.pokedex.pokeorts, 
                playerPosition = new Vector3(0, 0, 0), 
                inventory = Inventario.instance.items, 
                saveFile = file
            };

            string json = JsonUtility.ToJson(initialData);
            File.WriteAllText(path, json);

	        return LoadGame(file);
        }
	    else
	    {
	        return LoadGame(file);
	    }
    }

    public static SaveData LoadGame(string file) {
        string path = GetSavePath(file);
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json); 
        }

	    return null;
    }
}