using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item Data/Inventory")]
public class InventoryItemSO : ItemSO
{
    public enum ItemStatus
    {
        NORMAL,
        KEY_ITEM
    }

    public ItemStatus itemStatus = ItemStatus.NORMAL;

    public InventoryItemSO(string daName, string daDescription, Sprite daGraphic, ItemStatus daStatus)
    {
        itemName = daName;
        description = daDescription;
        graphic = daGraphic;
        itemStatus = daStatus;
    }
}
