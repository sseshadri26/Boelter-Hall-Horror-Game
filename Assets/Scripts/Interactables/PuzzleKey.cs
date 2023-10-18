using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour, IAction
{
    [SerializeField] private InventoryItemSO key;
    [SerializeField] private string message;

    private InventoryItemCollectionSO inventory;

    [SerializeField] GameObject OtherKey;

    [SerializeField] GameObject PortalPair;

    [SerializeField] GameObject ObjectsToEnable;

    private void Start()
    {
        inventory = Globals.inventory;

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

        if (OtherKey == null)
        {

            Globals.flags.Add("Floor5PortalRoomPortal", true);

            PortalPair.GetComponent<RotatingPortalDisableManager>().enabled = true;
            GameObject.FindWithTag("Player").GetComponent<BlackoutScreen>().flickerStart = true;
            ObjectsToEnable.SetActive(true);


        }

        Destroy(this.gameObject);


    }
}
