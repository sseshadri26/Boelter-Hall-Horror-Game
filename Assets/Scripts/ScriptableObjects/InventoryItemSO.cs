using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item")]
public class InventoryItemSO : ItemSO
{
    public enum ItemStatus
    {
        NORMAL,
        KEY_ITEM
    }

    public ItemStatus itemStatus = ItemStatus.NORMAL;
}
