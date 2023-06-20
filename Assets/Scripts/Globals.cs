using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static int curSpawnPoint = 0;
    public static int portalPosition5F = 0;
    public static bool playDoorCloseSoundAtNextScene = false;

    public static Dictionary<string, bool> flags = new Dictionary<string, bool>();
    public static InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

    public static void Reset()
    {
        curSpawnPoint = 0;
        portalPosition5F = 0;
        playDoorCloseSoundAtNextScene = false;
        flags = new Dictionary<string, bool>();
        inventory.items = new List<InventoryItemSO>();
    }
}
