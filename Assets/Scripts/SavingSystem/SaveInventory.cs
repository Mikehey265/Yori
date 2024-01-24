using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

public class SaveInventory : MonoBehaviour
{
    public static SaveInventory Instance { get; private set; }
    private string saveLocation;

    private void Awake()
    {
        saveLocation = Application.persistentDataPath + "/Inventory.json";
        Instance = this;
    }

    public void Save(Inventory inventory)
    {
        var jsonData = JsonUtility.ToJson(inventory);
        System.IO.File.WriteAllText(saveLocation, jsonData);
    }

    public Inventory Load()
    {
        string readJson = File.ReadAllText(saveLocation);
        Inventory inventory = JsonUtility.FromJson<Inventory>(readJson);
        return inventory;
    }
    
}
