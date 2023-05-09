using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For loading next scene on interact.
public class PortalKey : MonoBehaviour, IAction
{
    [SerializeField] private InventoryItemSO portalKey;

    // If portals are in orientation 1, then give player the key
    // If portals are in orientation 0, then remove key from player
    public void Activate()
    {
        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        if (inventory.items.Contains(portalKey))
        {
            inventory.items.Remove(portalKey);
            Notification.instance.ShowMessage("The strange key disappeared...");
        }
        else
        {
            inventory.items.Add(portalKey);
            Notification.instance.ShowMessage("A strange key appeared in your pocket...");
        }
    }
}
