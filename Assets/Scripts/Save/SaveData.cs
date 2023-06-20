using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    // We're using arrays and lists because they're serializable
    public int playerSpawnpoint;
    public string playerScene;

    public InventoryItemCollectionSO inventory;
    public List<string> itemNames = new List<string>();
    // public List<string> itemDescriptions = new List<string>();
    // public List<string> itemGraphics = new List<string>();
    // public List<InventoryItemSO.ItemStatus> itemStatuses = new List<InventoryItemSO.ItemStatus>();

    public List<string> flagKeys;
    public List<bool> flagValues;

    public int portalPosition = 0;

    // Create a SaveData from the static PlayerData class
    public void CopyFromGame()
    {
        SavePlayer();
        SaveInventory();
        SaveFlags();
        SaveMisc();
    }

    private void SavePlayer()
    {
        playerSpawnpoint = Globals.curSpawnPoint;

        playerScene = SceneManager.GetActiveScene().name;
    }

    private void SaveInventory()
    {
        InventoryItemCollectionSO inventory = Globals.inventory;

        foreach (InventoryItemSO item in inventory.items)
        {
            itemNames.Add(item.itemName);
            // itemDescriptions.Add(item.description);
            // itemGraphics.Add(item.graphic.name);
            // itemStatuses.Add(item.itemStatus);

            // Debug.Log("Adding " + item.itemName + " to save.");
        }
    }

    private void SaveFlags()
    {
        flagKeys = new List<string>(Globals.flags.Keys);
        flagValues = new List<bool>(Globals.flags.Values);

        // string debug = "Saving flags...";
        // foreach (string key in flagKeys)
        // {
        //     debug += "\n" + key;
        // }
        // Debug.Log(debug);
    }

    private void SaveMisc()
    {
        portalPosition = Globals.portalPosition5F;
    }
}