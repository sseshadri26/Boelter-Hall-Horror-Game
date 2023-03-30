using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item Collections/Inventory")]
public class InventoryItemCollectionSO : ScriptableObject
{
    // DESIGN CHOICE: Don't have a callback for when inventory updated,
    // since it's not something that's going to be changing very often,
    // and there are defined times when the inventory will need to be
    // updated/checked, meaning there won't be any polling anyways
    public List<InventoryItemSO> items;
}
