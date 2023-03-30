using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static SaveData data;

    public static void SaveGame()
    {
        data = new SaveData();
        data.CopyFromGame();

        string jsonData = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("MainSave", jsonData);
    }

    public static SaveData LoadGame()
    {
        return JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("MainSave", ""));
    }

    public static List<InventoryItemSO> GetInventory()
    {
        if (data == null)
        {
            data = LoadGame();
        }

        List<InventoryItemSO> loadedInventory = new List<InventoryItemSO>();
        SpriteAtlas itemSprites = Resources.Load<SpriteAtlas>("Items");

        for (int i = 0; i < data.itemNames.Count; i++)
        {
            loadedInventory.Add(new InventoryItemSO(data.itemNames[i], data.itemDescriptions[i], itemSprites.GetSprite(data.itemGraphics[i]), data.itemStatuses[i]));
        }

        return loadedInventory;
    }

    private static string Path()
    {
        return Application.persistentDataPath + "/save.boelter";
    }
}