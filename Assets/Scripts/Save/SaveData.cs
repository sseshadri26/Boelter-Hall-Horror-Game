using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    // We're using arrays and lists because they're serializable
    public float[] playerPos;
    public float[] playerRot;
    public string playerScene;

    public InventoryItemCollectionSO inventory;
    public List<string> itemNames = new List<string>();
    public List<string> itemDescriptions = new List<string>();
    public List<string> itemGraphics = new List<string>();
    public List<InventoryItemSO.ItemStatus> itemStatuses = new List<InventoryItemSO.ItemStatus>();

    public int portalPositions = 0;

    // Create a SaveData from the static PlayerData class
    public void CopyFromGame()
    {
        SavePlayer();
        SaveInventory();
        SavePortals();
    }

    private void SavePlayer()
    {
        Transform player = FirstPersonController.instance.transform;
        
        playerPos = new float[3];
        playerPos[0] = player.position.x;
        playerPos[1] = player.position.y;
        playerPos[2] = player.position.z;
        
        playerRot = new float[4];
        playerRot[0] = player.rotation.x;
        playerRot[1] = player.rotation.y;
        playerRot[2] = player.rotation.z;
        playerRot[3] = player.rotation.w;

        playerScene = SceneManager.GetActiveScene().name;
    }

    private void SaveInventory()
    {
        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        foreach (InventoryItemSO item in inventory.items)
        {
            itemNames.Add(item.itemName);
            itemDescriptions.Add(item.description);
            itemGraphics.Add(item.graphic.name);
            itemStatuses.Add(item.itemStatus);
        }
    }

    private void SavePortals()
    {
        portalPositions = Globals.portalPosition5F;
    }
}