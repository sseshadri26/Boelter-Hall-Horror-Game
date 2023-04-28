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
            if (_data == null)
            {
                _data = LoadGame();
                if (_data == null)
                {
                    Debug.Log("This is a new save");
                }
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
        SpriteAtlas itemSprites = Resources.Load<SpriteAtlas>("Items");

        for (int i = 0; i < Data.itemNames.Count; i++)
        {
            loadedInventory.Add(new InventoryItemSO(Data.itemNames[i], Data.itemDescriptions[i], itemSprites.GetSprite(Data.itemGraphics[i]), Data.itemStatuses[i]));
        }

        return loadedInventory;
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.boelter";
    }
}