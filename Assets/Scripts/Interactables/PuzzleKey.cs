using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour, IAction
{
    [SerializeField] private InventoryItemSO key;
    [SerializeField] private string message;
    public void Activate()
    {
        InventoryItemCollectionSO inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        inventory.items.Add(key);
        if (message != null && message != "")
        {
            Notification.instance.ShowMessage(message);
        }
        else
        {
            Notification.instance.ShowMessage("A gold key appeared in your pocket...");
        }
        Destroy(this.gameObject);
    }
}
