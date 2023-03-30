using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item")]
public class InventoryItemSO : ScriptableObject
{
    public string itemName;

    [TextArea(3, 10)]
    public string description;
    public Sprite graphic;

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
