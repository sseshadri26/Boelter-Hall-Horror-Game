using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static SaveData _data;
    public static SaveData Data
    {
        get
        {
            if (_data == null && !Globals.loaded)
            {
                _data = LoadGame();
                if (_data == null)
                {
                    Resources.Load<InventoryItemCollectionSO>("PlayerInventory").items = new List<InventoryItemSO>();
                    Debug.Log("This is a new save");
                }
                else
                {
                    Globals.portalPosition5F = _data.portalPosition;
                    Globals.curSpawnPoint = _data.playerSpawnpoint;
                    Resources.Load<InventoryItemCollectionSO>("PlayerInventory").items = GetInventory();
                    Globals.flags = GetFlags();
                    // foreach (KeyValuePair<string, bool> entry in Globals.flags)
                    // {
                    //     Debug.Log("Key: " + entry.Key + "\tValue: " + entry.Value);
                    // }
                }
                Globals.loaded = true;
            }
            return _data;
        }

        set
        {
            _data = value;
        }
    }

    private static SaveData LoadGame()
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("MainSave", ""));
    }

    public static void SaveGame()
    {
        Data = new SaveData();
        Data.CopyFromGame();

        string jsonData = JsonUtility.ToJson(Data);

        PlayerPrefs.SetString("MainSave", jsonData);
    }

    public static List<InventoryItemSO> GetInventory()
    {
        List<InventoryItemSO> loadedInventory = new List<InventoryItemSO>();
        // SpriteAtlas itemSprites = Resources.Load<SpriteAtlas>("Items");

        for (int i = 0; i < Data.itemNames.Count; i++)
        {
            loadedInventory.Add(Resources.Load<InventoryItemSO>("Inventory/" + Data.itemNames[i]));
            // loadedInventory[i].SetItem(Data.itemNames[i], Data.itemDescriptions[i], itemSprites.GetSprite(Data.itemGraphics[i]), Data.itemStatuses[i]);
        }

        return loadedInventory;
    }

    private static Dictionary<string, bool> GetFlags()
    {
        if (_data.flagKeys.Count != _data.flagValues.Count)
        {
            Debug.LogError("Somehow the number of flag keys and values grabbed is inequal!");
            return null;
        }

        Dictionary<string, bool> flagsDict = new Dictionary<string, bool>();
        for (int i = 0; i < _data.flagKeys.Count; i++)
        {
            flagsDict.Add(_data.flagKeys[i], _data.flagValues[i]);
            // Debug.Log("Loaded Key: " + _data.flagKeys[i] + "\tValue: " + _data.flagValues[i]);
        }

        return flagsDict;
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.boelter";
    }
}