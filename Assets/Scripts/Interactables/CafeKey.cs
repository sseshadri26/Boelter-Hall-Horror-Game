using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeKey : MonoBehaviour, IAction
{
    [SerializeField] private InventoryItemSO key;

    public void Activate() 
    {
        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        inventory.items.Add(key);
        Notification.instance.ShowMessage("A key appeared in your pocket...");
    }
}
