using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//For loading next scene on interact.
public class PortalKey : MonoBehaviour, IAction
{
    private bool activated = false;

    [SerializeField] private InventoryItemSO portalKey;

    // If portals are in orientation 1, then give player the key
    // If portals are in orientation 0, then remove key from player
    public void Activate()
    {
        // Prevent turning twice in a row
        if (activated)
        {
            return;
        }

        activated = true;
        // Jank way of waiting 1 sec before setting activated back to false (no rotation)
        transform.DOBlendableRotateBy(Vector3.zero, 1f).OnComplete(() => activated = false);

        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        if (inventory.items.Contains(portalKey))
        {
            inventory.items.Remove(portalKey);
            Notification.instance.ShowMessage("The air shifts around you. The strange key disappeared...");
        }
        else
        {
            inventory.items.Add(portalKey);
            Notification.instance.ShowMessage("The air shifts around you. A strange key appeared in your pocket...");
        }
    }
}
