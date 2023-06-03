using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

/// <summary> Static Yarn Spinner commands. Commands either return nothing or are coroutines </summary>
public static class YarnFunctions
{
    [YarnFunction("hasItem")]
    public static bool HasItem(string itemName)
    {
        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        foreach (InventoryItemSO item in inventory.items)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    [YarnFunction("flagIsTrue")]
    public static bool FlagIsTrue(string flagName)
    {
        // Debug.Log("Flag: " + flagName);
        // Debug.Log("Flag is in the dictionary: " + Globals.flags.ContainsKey(flagName));
        return Globals.flags.ContainsKey(flagName) && Globals.flags[flagName];
    }
}
