using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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

    public static SaveData LoadGame(Button boton) {
        string path = GetSavePath(boton.tag);
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json); 
        }
        return null;
    }
}