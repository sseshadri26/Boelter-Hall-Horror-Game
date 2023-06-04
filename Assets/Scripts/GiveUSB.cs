using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUSB : MonoBehaviour
{
    private void Start()
    {
        var inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory").items = new List<InventoryItemSO>();

        InventoryItemSO usb = Resources.Load<InventoryItemSO>("Inventory/USB Drive");
        if (usb != null && !inventory.Contains(usb))
        {
            inventory.Add(usb);
        }
    }
}
