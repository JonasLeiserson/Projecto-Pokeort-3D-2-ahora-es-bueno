using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public Pokedex pokedex;
    public Vector3 playerPosition;
    public Inventario inventory;
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

    public static void InitSaveFileIfNeeded(string file) {
        string path = GetSavePath(file);
        if (!File.Exists(path)) {
	    GameManager.instance.data.pokedex.pokeorts.Clear();
            Inventario.instance.items.Clear();
            SaveData initialData = new SaveData { pokedex = GameManager.instance.pokedex, playerPosition = new Vector3(0, 0, 0), inventory = Inventario.instance, saveFile = file};
            string json = JsonUtility.ToJson(initialData);
            File.WriteAllText(path, json);
	        LoadGame(file);
        }
	    else
	    {
	        LoadGame(file);
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