using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUSB : MonoBehaviour
{
    private void Start()
    {
        InventoryItemCollectionSO inventory = Globals.inventory;

        InventoryItemSO usb = Resources.Load<InventoryItemSO>("Inventory/USB Drive");
        if (usb != null && !inventory.items.Contains(usb))
        {
            inventory.items.Add(usb);
        }
    }
}
