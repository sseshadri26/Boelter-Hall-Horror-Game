using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static SaveData data;

    public static void LoadGame()
    {
        data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("MainSave", ""));

        Globals.portalPosition5F = data.portalPosition;
        Globals.curSpawnPoint = data.playerSpawnpoint;
        Globals.inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");
        Globals.inventory.items = GetInventory();
        // foreach (InventoryItemSO item in Globals.inventory.items)
        // {
        //     Debug.Log("Item: " + item);
        // }
        Globals.flags = GetFlags();
        // foreach (KeyValuePair<string, bool> entry in Globals.flags)
        // {
        //     Debug.Log("Key: " + entry.Key + "\tValue: " + entry.Value);
        // }
    }

    public static void SaveGame()
    {
        data = new SaveData();
        data.CopyFromGame();

        string jsonData = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("MainSave", jsonData);
    }

    private static List<InventoryItemSO> GetInventory()
    {
        // Debug.Log("GetInventory called. Item count: " + data.itemNames.Count);
        List<InventoryItemSO> loadedInventory = new List<InventoryItemSO>();
        // SpriteAtlas itemSprites = Resources.Load<SpriteAtlas>("Items");

        for (int i = 0; i < data.itemNames.Count; i++)
        {
            InventoryItemSO item = Resources.Load<InventoryItemSO>("Inventory/" + data.itemNames[i]);

            if (item == null)
            {
                Debug.LogError("Could not find " + data.itemNames[i] + " in \"Assets/Resources/Inventory\"! Make sure it has the same filename as the item's name.");
            }
            loadedInventory.Add(Resources.Load<InventoryItemSO>("Inventory/" + data.itemNames[i]));
            // loadedInventory[i].SetItem(_data.itemNames[i], _data.itemDescriptions[i], itemSprites.GetSprite(_data.itemGraphics[i]), _data.itemStatuses[i]);
        }

        return loadedInventory;
    }

    private static Dictionary<string, bool> GetFlags()
    {
        if (data.flagKeys.Count != data.flagValues.Count)
        {
            Debug.LogError("Somehow the number of flag keys and values grabbed is inequal!");
            return null;
        }

        Dictionary<string, bool> flagsDict = new Dictionary<string, bool>();
        for (int i = 0; i < data.flagKeys.Count; i++)
        {
            flagsDict.Add(data.flagKeys[i], data.flagValues[i]);
            // Debug.Log("Loaded Key: " + _data.flagKeys[i] + "\tValue: " + _data.flagValues[i]);
        }

        return flagsDict;
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.boelter";
    }

    public static bool HasSavedgame()
    {
        return PlayerPrefs.HasKey("MainSave");
    }

    public static string GetLoadedScene()
    {
        if (!HasSavedgame() || data == null)
        {
            Debug.LogError("No saved game or game wasn't loaded!");
            return "Intro";
        }

        return data.playerScene;
    }
}