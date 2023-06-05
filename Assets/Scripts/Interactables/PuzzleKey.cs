using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour, IAction
{
    [SerializeField] private InventoryItemSO key;
    [SerializeField] private string message;

    private InventoryItemCollectionSO inventory;

    private void Start()
    {
        inventory = Resources.Load<InventoryItemCollectionSO>("PlayerInventory");

        // If we already have the key, destroy it
        if (inventory.items.Contains(key))
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
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
